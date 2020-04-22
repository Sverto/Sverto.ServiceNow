using Newtonsoft.Json;
using Sverto.ServiceNow.API;

namespace Sverto.ServiceNow.Models
{
    public class Group : Record
    {
        public new static string TABLE_NAME { get; } = "sys_user_group";

        [JsonProperty("name")] // CMDB Admin
        public string Name { get; set; }

        [JsonProperty("u_code")] // CMDB
        public string Code { get; set; }

        [JsonProperty("u_coordinator.name")] // Sebastien Cornet
        public string Coordinator { get; set; }

        [JsonProperty("u_level")] // 3rd line
        public string SupportLevel { get; set; }
    }
}
