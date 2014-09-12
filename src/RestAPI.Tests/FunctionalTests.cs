﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TwitterOAuth.RestAPI;
using TwitterOAuth.RestAPI.Models;
using TwitterOAuth.RestAPI.Resources;

namespace RestAPI.Tests
{
    [TestFixture]
    public class FunctionalTests
    {
        [Test]
        public async Task CreateAuthHeaderAndTestWithHttpRequest()
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
            var uri = new Uri(string.Format("{0}?{1}", Urls.SearchUrl, query));

            var authHeader = authorization.GetHeader(uri);

            Assert.IsNotNull(authHeader);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authHeader);

                var request = new HttpRequestMessage
                {
                    RequestUri = uri,
                    Method = HttpMethod.Get
                };

                var responseMessage = await httpClient.SendAsync(request).ConfigureAwait(false);

                Assert.IsNotNull(responseMessage);
                Assert.IsTrue(responseMessage.IsSuccessStatusCode);

                var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jObject = JObject.Parse(response);
                var responseObject = JsonConvert.DeserializeObject<dynamic>(jObject.ToString());

                Assert.IsNotNull(responseObject);
            }
        }
    }
}
