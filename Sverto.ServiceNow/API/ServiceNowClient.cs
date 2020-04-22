using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Sverto.ServiceNow.Helpers;
using Sverto.ServiceNow.ValueTypes;
using System.Linq;
using Mono.Web;

namespace Sverto.ServiceNow.API
{
    /// <summary>
    /// API client that uses ServiceNow Table API to retrieve records or create them
    /// </summary>
    public class ServiceNowClient : IDisposable
    {
        #region Init & Dispose
        protected HttpClient _Client;
        public string Instance { get; }

        /// <summary>
        /// Create a ServiceNowClient instance
        /// </summary>
        /// <param name="instance">Servicenow instance ([INSTANCE].service-now.com).</param>
        /// <param name="credential">Basic Auth username and password NetworkCredential.</param>
        public ServiceNowClient(string instance, NetworkCredential credential)
        {
            if (string.IsNullOrWhiteSpace(instance))
                throw new ArgumentNullException(nameof(instance));
            if (credential == null)
                throw new ArgumentNullException(nameof(credential));

            _Client = new HttpClient(new HttpClientHandler()
            {
                Credentials = credential,
                DefaultProxyCredentials = CredentialCache.DefaultCredentials,
            })
            {
                BaseAddress = new Uri($"https://{instance}.service-now.com/api/now/")
            };
            Instance = instance;

            // Use TLS 1.2 by default
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        public virtual void Dispose()
        {
            _Client?.Dispose();
        }
        #endregion


        #region Base
        protected virtual string GetUrl<T>() where T : Record
        {
            return "table/" + RecordTable.GetTableName<T>();
        }

        protected virtual async Task<T> Get<T>(string url) where T : RestResponse, new()
        {
            var response = await _Client.GetAsJsonAsync(url);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<T>();
            else
                return new T() { ErrorMsg = $"GET error ({response.StatusCode}): {response.ReasonPhrase}" };
        }

        protected virtual async Task<T> Post<T, R>(string url, R record) where T : RestResponse, new() where R : Record
        {
            var json = JsonConvert.SerializeObject(record, new JsonSerializerSettings()
                { NullValueHandling = NullValueHandling.Ignore, Converters = { new ResourceLinkJsonConverter() } });

            var response = await _Client.PostAsJsonAsync(url, json);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<T>();
            else
                return new T() { ErrorMsg = $"POST error ({response.StatusCode}): {response.ReasonPhrase}" };
        }
        #endregion


        #region API Call Methods
        /// <summary>
        /// Get ServiceNow record of type T (which must inherit from Record) by id 
        /// </summary>
        /// <typeparam name="T">Record type (example: SupportRequest)</typeparam>
        /// <param name="id">ServiceNow sys_id</param>
        /// <returns></returns>
        public Task<RestResponseSingle<T>> GetById<T>(string id) where T : Record
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));
            if (!RecordNumber.IsValidId(id))
                throw new ArgumentException("Invalid record ID.", nameof(id));

            return Get<RestResponseSingle<T>>($"{GetUrl<T>()}/{id}?sysparm_limit=1&sysparm_fields={RecordTable.GetFieldList<T>()}");
        }

        /// <summary>
        /// Get ServiceNow records of type T (which must inherit from Record) by a ServiceNow string query
        /// The maximum number of records returned is 100
        /// </summary>
        /// <typeparam name="T">Record type (example: SupportRequest)</typeparam>
        /// <param name="query">ServiceNow string query (example: "number=SR1234567")</param>
        /// <returns></returns>
        public async Task<RestResponseQuery<T>> GetByQuery<T>(string query) where T : Record
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var response = await Get<RestResponseQuery<T>>($"{GetUrl<T>()}?sysparm_limit=100&sysparm_fields={RecordTable.GetFieldList<T>()}&sysparm_query={query}");
            if (!response.IsError && response.Result.Count == 0)
                response.ErrorMsg = "GET error (NotFound): Empty list";

            return response;
        }

        /// <summary>
        /// Get ServiceNow record of type T (which must inherit from Record) by its number
        /// </summary>
        /// <typeparam name="T">Record type (example: SupportRequest)</typeparam>
        /// <param name="number">Instance of RecordNumber (example: new RecordNumber("SR1234567"))</param>
        /// <returns></returns>
        public async Task<RestResponseSingle<T>> GetByNumber<T>(RecordNumber number) where T : Record
        {
            if (number == null)
                throw new ArgumentNullException(nameof(number));

            var result = await GetByQuery<T>(@"number=" + number.Number);
            return new RestResponseSingle<T>() { ErrorMsg = result.ErrorMsg, Result = result.Result?.FirstOrDefault() };
        }

        /// <summary>
        /// Get ServiceNow child records by parent RecordNumber
        /// The maximum number of records returned is 5000
        /// </summary>
        /// <typeparam name="T">Record type</typeparam>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task<RestResponseQuery<T>> GetByParentNumber<T>(RecordNumber number) where T : Record
        {
            if (number == null)
                throw new ArgumentNullException(nameof(number));

            return await Get<RestResponseQuery<T>>($"{GetUrl<T>()}?sysparm_limit=5000&sysparm_fields={RecordTable.GetFieldList<T>()}&sysparm_query=parent.number={number.Number}");
        }

        /// <summary>
        /// Create new record in ServiceNow using its Table API
        /// </summary>
        /// <typeparam name="T">Record type</typeparam>
        /// <param name="record">Instance of Record type</param>
        /// <returns></returns>
        public Task<RestResponseSingle<T>> Post<T>(T record) where T : Record, new()
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));
            if (record.Equals(new T()))
                throw new ArgumentException("The record cannot be empty.", nameof(record));
            if (record.Id != null)
                throw new ArgumentException("The record cannot contain an ID when creating.", nameof(record));

            return Post<RestResponseSingle<T>, T>($"{GetUrl<T>()}?sysparm_fields={RecordTable.GetFieldList<T>()}", record);
        }

        /// <summary>
        /// Add a binary attachment to existing record
        /// Can be file/image/mail/... depending on what ServiceNow allows
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recordId">Record sys_id</param>
        /// <param name="filename">Name of the attachment</param>
        /// <param name="file">The attachment in binary format</param>
        /// <returns></returns>
        public async Task<RestResponseSingle<Attachment>> PostAttachment<T>(string recordId, string filename, byte[] file) where T : Record
        {
            if (string.IsNullOrWhiteSpace(recordId))
                throw new ArgumentNullException(nameof(recordId));
            if (!RecordNumber.IsValidId(recordId))
                throw new ArgumentException("Invalid record ID.", nameof(recordId));
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException(nameof(filename));
            if (file == null || file.Length == 0)
                throw new ArgumentNullException(nameof(file));

            // Convert to safe values
            var nvs = new NameValueCollection
            {
                { "table_name",   RecordTable.GetTableName<T>() },
                { "table_sys_id", recordId },
                { "file_name",    HttpUtility.UrlEncode(filename) }
            };

            // Build url with keys
            var urlBuilder = new StringBuilder();
            urlBuilder.Append("attachment/file?");
            for (var i = 0; i < nvs.Keys.Count; i++)
            {
                if (i > 0)
                    urlBuilder.Append("&");
                var key = nvs.Keys[i];
                urlBuilder.AppendFormat("{0}={1}", key, nvs[key]);
            }
            var url = urlBuilder.ToString();

            // Post
            var response = await _Client.PostAsFileAsync(url, filename, file);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsJsonAsync<RestResponseSingle<Attachment>>();
            else
                return new RestResponseSingle<Attachment>() { ErrorMsg = $"POST attachment error ({response.StatusCode}): {response.ReasonPhrase}" };
        }
        #endregion

    }
}
