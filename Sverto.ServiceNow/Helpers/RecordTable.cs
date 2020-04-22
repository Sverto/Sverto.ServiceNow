using Sverto.ServiceNow.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sverto.ServiceNow.Helpers
{
    public static class RecordTable
    {
        private static Dictionary<Type, string> _FieldLists = new Dictionary<Type, string>();

        public static string GetTableName(Type type)
        {
            // C# can't pass type parameters as 'of type' ... workarround
            if (!type.GetTypeInfo().IsSubclassOf(typeof(Record)))
                throw new ArgumentException("The given type is not a member of " + nameof(Record), nameof(type));

            var tableName = (string)type.GetProperty("TABLE_NAME").GetValue(null);

            if (string.IsNullOrEmpty(tableName))
                throw new InvalidOperationException("Field TABLE_NAME is not set in Record class");

            return tableName;
        }

        public static string GetTableName<T>() where T : Record
        {
            return GetTableName(typeof(T));
        }

        internal static string GetFieldList<T>() where T : Record
        {
            Type t = typeof(T);
            string fieldList;

            // Search cached fieldlists
            if (_FieldLists.TryGetValue(t, out fieldList))
            {
                return fieldList;
            }

            // Build fieldlist
            fieldList = "";
            foreach (var prop in t.GetProperties())
            {
                // We need to build the field list using the JsonProperty attributes since those strings can contain our dot notation.
                var field = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "JsonPropertyAttribute");
                if (field != null)
                {
                    var fieldName = field.ConstructorArguments.FirstOrDefault(x => x.ArgumentType.Name == "String");
                    if (fieldName.Value != null)
                    {
                        if (fieldList.Length > 0) { fieldList += ","; }
                        fieldList += fieldName.Value;
                    }
                }
            }
            _FieldLists.Add(t, fieldList);
            return fieldList;
        }
    }
}
