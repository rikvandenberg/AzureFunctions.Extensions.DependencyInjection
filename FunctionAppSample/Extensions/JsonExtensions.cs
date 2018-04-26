using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FunctionAppSample
{
    internal static class JsonExtensions
    {
        internal static HttpResponseMessage CreateJsonResponse<T>(HttpStatusCode status, T value)
        {
            var jsonToReturn = value.ToJson();

            return new HttpResponseMessage(status)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
        }

        internal static string ToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
