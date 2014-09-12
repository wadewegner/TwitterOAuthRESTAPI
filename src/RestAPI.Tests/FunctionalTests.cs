using System.Net.Http;
using System.Net.Http.Headers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterOAuth.RestAPI;
using TwitterOAuth.RestAPI.Models;

namespace RestAPI.Tests
{
    [TestFixture]
    public class FunctionalTests
    {
        [Test]
        public async Task Test()
        {
            var secret = new Secret
            {
                ApiKey = "",
                ApiSecret = "",
                AccessToken = "",
                AccessTokenSecret = ""
            };

            var authorization = new Authorization(secret);

            const string query = "q=wadewegner";
            var url = string.Format("{0}?{1}", Resources.SearchUrl, query);

            var authHeader = authorization.GetHeader(Resources.SearchUrl, query);

            Assert.IsNotNull(authHeader);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authHeader);

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get //TODO: dymanic
                };

                var responseMessage = await httpClient.SendAsync(request).ConfigureAwait(false);

                Assert.IsNotNull(responseMessage);
                Assert.IsTrue(responseMessage.IsSuccessStatusCode);
            }
        }
    }
}
