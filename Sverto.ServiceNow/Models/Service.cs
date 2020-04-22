using Newtonsoft.Json;
using Sverto.ServiceNow.API;

namespace Sverto.ServiceNow.Models
{
    public class Service : Record
    {
        public new static string TABLE_NAME { get; } = "service_offering";

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
