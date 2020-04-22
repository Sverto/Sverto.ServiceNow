using System.Collections.Generic;

namespace Sverto.ServiceNow.API
{
    public abstract class RestResponse
    {
        public string ErrorMsg { get; set; }
        public bool IsError => (ErrorMsg?.Length > 0);
    }

    public class RestResponseSingle<T> : RestResponse
    {
        public T Result { get; set; }
    }

    public class RestResponseQuery<T> : RestResponse
    {
        public ICollection<T> Result { get; set; } = new List<T>();
    }
}
