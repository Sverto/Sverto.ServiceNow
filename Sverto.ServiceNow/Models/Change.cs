using Newtonsoft.Json;
using Sverto.ServiceNow.API;

namespace Sverto.ServiceNow.Models
{
    public class Change : Record
    {
        public new static string TABLE_NAME { get; } = "u_change_request";

        [JsonProperty("number")]
        public string Number { get; set; }
    }
}
