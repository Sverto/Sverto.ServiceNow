using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sverto.ServiceNow.Helpers
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> GetAsJsonAsync(this HttpClient httpClient, string url)
        {
            // Seems .NET Core has more to offer than Framework...
#if (NETCORE)
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = new StringContent(string.Empty, Encoding.UTF8, "application/json")
            };
            return httpClient.SendAsync(httpRequest);
#else
            return httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
#endif
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient httpClient, string url, string data)
        {
            return httpClient.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"));
        }

        public static Task<HttpResponseMessage> PostAsFileAsync(this HttpClient httpClient, string url, string filename, byte[] file)
        {
            var content = new ByteArrayContent(file);
            content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(Path.GetExtension(filename)));
            return httpClient.PostAsync(url, content);
        }

        public async static Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
