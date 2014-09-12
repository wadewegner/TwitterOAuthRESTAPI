using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwitterOAuth.RestAPI.Models;

namespace TwitterOAuth.RestAPI
{
    public class Authorization
    {
        public Secret Secret { get; private set; }

        public Authorization(Secret secret)
        {
            Secret = secret;
        }

        public string GetHeader(string resourceUrl, string query)
        {
            const string oauthVersion = "1.0";
            const string oauthSignatureMethod = "HMAC-SHA1";

            var oauthNonce = Guid.NewGuid().ToString();
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauthTimestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString(CultureInfo.InvariantCulture);

            const string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                                        "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&q={6}";

            var baseString = string.Format(baseFormat,
                                        Secret.ApiKey,
                                        oauthNonce,
                                        oauthSignatureMethod,
                                        oauthTimestamp,
                                        Secret.AccessToken,
                                        oauthVersion,
                                        Uri.EscapeDataString(query)
                                        );

            baseString = string.Concat("GET&",
                Uri.EscapeDataString(resourceUrl), "&",
                Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(Secret.ApiSecret),
                                    "&", Uri.EscapeDataString(Secret.AccessTokenSecret));

            string oauthSignature;
            using (var hasher = new HMACSHA1(Encoding.ASCII.GetBytes(compositeKey)))
            {
                oauthSignature = Convert.ToBase64String(
                    hasher.ComputeHash(Encoding.ASCII.GetBytes(baseString)));
            }

            const string headerFormat = "oauth_consumer_key=\"{0}\", " +
                                        "oauth_nonce=\"{1}\", " +
                                        "oauth_signature=\"{2}\", " +
                                        "oauth_signature_method=\"{3}\", " +
                                        "oauth_timestamp=\"{4}\", " +
                                        "oauth_token=\"{5}\", " +
                                        "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(Secret.ApiKey),
                                    Uri.EscapeDataString(oauthNonce),
                                    Uri.EscapeDataString(oauthSignature),
                                    Uri.EscapeDataString(oauthSignatureMethod),
                                    Uri.EscapeDataString(oauthTimestamp),
                                    Uri.EscapeDataString(Secret.AccessToken),
                                    Uri.EscapeDataString(oauthVersion)
                            );

            return authHeader;
        }

        private static WebRequest Sign(WebRequest request, string appKey, string appSecret, string token, string tokenSecret)
        {
            var nonce = Guid.NewGuid().ToString();
            var timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();

            var oauthParameters = new SortedDictionary<string, string>();
            oauthParameters.Add("oauth_consumer_key", appKey);
            oauthParameters.Add("oauth_nonce", nonce);
            oauthParameters.Add("oauth_signature_method", "HMAC-SHA1");
            oauthParameters.Add("oauth_timestamp", timestamp);
            oauthParameters.Add("oauth_token", token);
            oauthParameters.Add("oauth_version", "1.0");

            var signingParameters = new SortedDictionary<string, string>(oauthParameters);

            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            foreach (var k in query.AllKeys)
            {
                signingParameters.Add(k, query[k]);
            }

            var builder = new UriBuilder(request.RequestUri);
            builder.Query = "";
            var baseUrl = builder.Uri.AbsoluteUri;

            var parameterString = string.Join("&",
                                    from k in signingParameters.Keys
                                    select Uri.EscapeDataString((k)) + "=" +
                                           Uri.EscapeDataString(signingParameters[k]));

            var stringToSign = string.Join("&", from s in new string[] { request.Method.ToUpper(), baseUrl, parameterString }
                                                select Uri.EscapeDataString(s));

            Console.WriteLine(stringToSign);

            var signingKey = appSecret + "&" + tokenSecret;

            var signature = Convert.ToBase64String(new HMACSHA1(Encoding.ASCII.GetBytes(signingKey)).ComputeHash(Encoding.ASCII.GetBytes(stringToSign)));

            Console.WriteLine(signature);

            oauthParameters.Add("oauth_signature", signature);

            var authHeader = "OAuth " + string.Join(", ", from k in oauthParameters.Keys
                                                          select string.Format(@"{0}=""{1}""",
                                                            Uri.EscapeDataString((k)),
                                                            Uri.EscapeDataString(oauthParameters[k])));

            Console.WriteLine(authHeader);

            request.Headers.Add(HttpRequestHeader.Authorization, authHeader);

            return request;
        }

    }
}
