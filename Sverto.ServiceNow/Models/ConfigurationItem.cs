using Newtonsoft.Json;
using Sverto.ServiceNow.API;

namespace Sverto.ServiceNow.Models
{
    public class ConfigurationItem : Record
    {
        public new static string TABLE_NAME { get; } = "cmdb_ci_appl";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("u_code")]
        public string Code { get; set; }

        [JsonProperty("u_environment")]
        public string Environment { get; set; }

        [JsonProperty("support_group_label")]
        public string SupportGroup { get; set; }
    }
}
