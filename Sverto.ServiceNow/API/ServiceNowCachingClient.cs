using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Sverto.ServiceNow.API
{
    /// <summary>
    /// ServiceNowClient with response cache to reduce API calls
    /// </summary>
    public class ServiceNowCachingClient : ServiceNowClient
    {
        #region Base Override
        /// <summary>
        /// Create a ServiceNowClient instance
        /// </summary>
        /// <param name="instance">Servicenow instance ([INSTANCE].service-now.com).</param>
        /// <param name="credential">Basic Auth username and password NetworkCredential.</param>
        public ServiceNowCachingClient(string instance, NetworkCredential credential) : base(instance, credential)
        {
        }

        protected override async Task<T> Get<T>(string url)
        {
            // Get from cache
            var cachedResponse = GetCachedResponse(url);
            if (cachedResponse != null)
                return cachedResponse as T ?? throw new InvalidOperationException("RestResponse must be of type T.");

            // Get from ServiceNow & add to cache
            var response = await base.Get<T>(url);
            if (!response.IsError)
                AddCachedResponse(url, response);

            return response;
        }

        public override void Dispose()
        {
            base.Dispose();
            _Cache.Clear();
            _Cache = null;
        }
        #endregion


        #region Cache
        private Dictionary<string, RestResponse> _Cache = new Dictionary<string, RestResponse>();

        private RestResponse GetCachedResponse(string requestUrl)
        {
            _Cache.TryGetValue(requestUrl, out var value);
            return value;
        }

        private void AddCachedResponse(string requestUrl, RestResponse response)
        {
            if (!_Cache.ContainsKey(requestUrl))
                _Cache.Add(requestUrl, response);
        }

        public void ClearCache()
        {
            _Cache.Clear();
        }
        #endregion

    }
}
