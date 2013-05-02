using System.Net.Http;
using Newtonsoft.Json;

namespace Procent.Redirector.Tests.Acceptance
{
    public static class HttpResponseExtentions
    {
        public static T DeserializeAsJson<T>(this HttpResponseMessage @this)
        {
            return JsonConvert.DeserializeObject<T>(@this.Content.ReadAsStringAsync().Result);
        }
    }
}