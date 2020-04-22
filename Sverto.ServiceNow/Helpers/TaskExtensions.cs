using System.Linq;
using System.Threading.Tasks;
using Sverto.ServiceNow.API;

namespace Sverto.ServiceNow.Helpers
{
    public static class TaskExtensions
    {
        public static async Task<T> Resolve<T>(this Task<RestResponseQuery<T>> task) where T : Record
        {
            var result = await task;
            return result.IsError ? null : result.Result.First();
        }

        public static async Task<T> Resolve<T>(this Task<RestResponseSingle<T>> task) where T : Record
        {
            var result = await task;
            return result.IsError ? null : result.Result;
        }

        public static async Task<ResourceLink> ToResourceLink<T>(this Task<T> task) where T : Record
        {
            var result = await task;
            return result == null ? null : new ResourceLink(result.Id);
        }
    }
}
