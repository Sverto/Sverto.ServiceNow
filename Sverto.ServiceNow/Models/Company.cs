using Newtonsoft.Json;
using Sverto.ServiceNow.API;

namespace Sverto.ServiceNow.Models
{
    public class Company : Record
    {
        public new static string TABLE_NAME { get; } = "core_company";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("notes")]
        public string Description { get; set; }
    }
}
