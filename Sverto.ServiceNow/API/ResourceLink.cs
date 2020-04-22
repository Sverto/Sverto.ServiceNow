using Newtonsoft.Json;

namespace Sverto.ServiceNow.API
{
    //[JsonConverter(typeof(ResourceLinkJsonConverter))]
    public class ResourceLink
    {
        public ResourceLink()
        {
        }

        public ResourceLink(string value)
        {
            Value = value;
        }

        [JsonProperty("link")]
        public string Link { get; set; }    // REST URL for child record

        [JsonProperty("value")]
        public string Value { get; set; }   // Reference to the child record (sys_id)

        // The ToString Method is called on serialization back to servicenow through the ResourceLinkConverter
        public override string ToString() { return Value; }
    }
}
