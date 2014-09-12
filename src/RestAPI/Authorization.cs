using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

            var oauthNonce = Uri.EscapeDataString(Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture))));
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
    }
}
