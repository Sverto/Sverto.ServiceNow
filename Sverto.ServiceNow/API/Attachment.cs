using Newtonsoft.Json;

namespace Sverto.ServiceNow.API
{
    /// <summary>
    /// Attachment to add to a specific record type
    /// </summary>
    public class Attachment
    {
        [JsonProperty("table_sys_id")]
        public string RecordId { get; set; }

        [JsonProperty("table_name")]
        public string RecordTableName { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("size_bytes")] // ex. 36597
        public string FileSize { get; set; }

        [JsonProperty("content_type")] // ex. image/jpeg
        public string ContentType { get; set; }

        [JsonProperty("download_link")] // ex. "https://instance.service-now.com/api/now/attachment/6ea10fe64f411200adf9f8e18110c739/file"
        public string DownloadLink { get; set; }
    }
}
