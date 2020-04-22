using Newtonsoft.Json;
using Sverto.ServiceNow.API;

namespace Sverto.ServiceNow.Models
{
    public class Problem : Record
    {
        public new static string TABLE_NAME { get; } = "u_problem";

        [JsonProperty("number")]
        public string Number { get; set; }
    }
}
