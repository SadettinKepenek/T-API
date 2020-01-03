using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace T_API.UI.Extensions
{
    public static class HttpRequestExtensions
    {
        public static async Task<JObject> ConvertRequestBody(this HttpRequest request)
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            body = body.Trim('"');
            JObject jObject = JObject.FromObject(JsonConvert.DeserializeObject(body));
            return jObject;
        }
    }
}