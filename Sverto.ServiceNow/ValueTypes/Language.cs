using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Sverto.ServiceNow.ValueTypes
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Language
    {
        [EnumMember(Value = "en")] English,
        [EnumMember(Value = "fr")] French,
        [EnumMember(Value = "nl")] Dutch,
        [EnumMember(Value = "de")] German
    }
}
