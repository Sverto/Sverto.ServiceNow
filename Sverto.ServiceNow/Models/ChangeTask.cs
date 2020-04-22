using Newtonsoft.Json;
using Sverto.ServiceNow.API;

namespace Sverto.ServiceNow.Models
{
    public class ChangeTask : Record
    {
        public new static string TABLE_NAME { get; } = "u_change_task";

        [JsonProperty("number")]
        public string Number { get; set; }
    }
}
