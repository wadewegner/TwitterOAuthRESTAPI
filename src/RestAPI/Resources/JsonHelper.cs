using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwitterOAuth.RestAPI.Resources
{
    public class JsonHelper
    {
        public static T ParseResponseContent<T>(string responseContent)
        {
            var jToken = JToken.Parse(responseContent);
            if (jToken.Type == JTokenType.Array)
            {
                var jArray = JArray.Parse(responseContent);

                T r = JsonConvert.DeserializeObject<T>(jArray.ToString());
                return r;
            }
            else
            {
                var jObject = JObject.Parse(responseContent);

                T r = JsonConvert.DeserializeObject<T>(jObject.ToString());
                return r;
            }
        }
    }
}