using Newtonsoft.Json;
using Sverto.ServiceNow.API;

namespace Sverto.ServiceNow.Models
{
    /// <summary>
    /// ServiceNow user model
    /// </summary>
    public class User : Record
    {
        public new static string TABLE_NAME { get; } = "sys_user";

        [JsonProperty("employee_number")]
        public string EmployeeNumber { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("user_name")]
        public string UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("u_windows_user_id")]
        public string WindowsUser { get; set; }

        [JsonProperty("company")]
        public ResourceLink Company { get; set; }

        [JsonProperty("u_works_for")]
        public ResourceLink WorksForCompany { get; set; }
    }
}
