using System;
using System.Linq;
using System.Web.Routing;

namespace TwitterOAuth.RestAPI.Resources
{
    public class UriHelper
    {
        public static string ConvertDynamicToQueryStringParameters(dynamic parameters)
        {
            var d = new RouteValueDictionary(parameters);

            // TODO: support query string array value 
            var queryString = string.Join("&",
                from i in d
                where
                    !String.IsNullOrWhiteSpace(i.Key)
                    && (i.Value != null && !String.IsNullOrWhiteSpace(i.Value.ToString()))
                select
                    Uri.EscapeDataString(i.Key)
                    + "="
                    + Uri.EscapeDataString(i.Value.ToString()));

            return queryString;
        }
    }
}