using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Sverto.ServiceNow.ValueTypes
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContactType
    {
        [EnumMember(Value = "phone")] Phone,
        [EnumMember(Value = "portal")] ServicePortal,
        [EnumMember(Value = "event")] Event,
        [EnumMember(Value = "observation")] OwnObservation,
        [EnumMember(Value = "walk")] WalkBy,
        [EnumMember(Value = "email")] Email,
        [EnumMember(Value = "fax")] Fax,
        [EnumMember(Value = "social_media")] SocialMedia,
        [EnumMember(Value = "chat")] Chat
    }
}
