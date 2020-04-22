using System.Linq;
using Newtonsoft.Json;

namespace Sverto.ServiceNow.API
{
    /// <summary>
    /// Base table for a service now record.  All records should include this at minimum
    /// </summary>
    public abstract class Record
    {
        [JsonProperty("sys_id")]
        public string Id { get; set; }

        // ServiceNow Tablename
        [JsonIgnore]
        public static string TABLE_NAME { get; }


        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            // Compare by reference and type
            if (ReferenceEquals(this, obj))
                return true;
            if ((obj as Record) == null || GetType() != obj.GetType())
                return false;

            // Compare by equality (GetType will get the most derived type)
            return GetType().GetProperties()
                .All(p => p.GetValue(this) == obj.GetType().GetProperty(p.Name).GetValue(obj));
        }
    }
}
