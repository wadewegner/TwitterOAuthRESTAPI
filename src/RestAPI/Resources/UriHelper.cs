using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace TwitterOAuth.RestAPI.Resources
{
    public class UriHelper
    {
        public static string ConvertDynamicToQueryStringParameters(dynamic parameters)
        {
            var d = new RouteValueDictionary(parameters);
            var sb = new StringBuilder();

            foreach (var i in d)
            {
                sb.AppendFormat("{0}={1}&", i.Key, i.Value);
            }

            return sb.ToString().TrimEnd('&');
        }
    }
}
