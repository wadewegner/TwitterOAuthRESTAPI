using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TwitterOAuth.RestAPI;
using TwitterOAuth.RestAPI.Models;
using TwitterOAuth.RestAPI.Models.Twitter;
using TwitterOAuth.RestAPI.Resources;

namespace RestAPI.Tests
{
    [TestFixture]
    public class FunctionalTests
    {
        private SecretModel _secretModel;
        private Authorization _authorization;
        private static HttpClient _httpClient;

        [TestFixtureSetUp]
        public void Init()
        {
            _secretModel = new SecretModel
            {
                ApiKey = "",
                ApiSecret = "",
                AccessToken = "",
                AccessTokenSecret = ""
            };

            _authorization = new Authorization(_secretModel);
            _httpClient = new HttpClient();
        }

        [TestFixtureTearDown]
        public void Cleanup()
        { /* ... */ }

        [Test]
        public async Task SearchTweets()
        {
            var uri = new Uri(string.Format("{0}?{1}", Urls.SearchTweets, "q=wadewegner"));

            var authHeader = _authorization.GetHeader(uri);
            Assert.IsNotNull(authHeader);

            await HttpSend<SearchTweetsModel>(authHeader, uri);
        }

        [Test]
        public async Task FriendsIds()
        {
            var uri = new Uri(string.Format("{0}?{1}", Urls.FriendsIds, "screen_name=wadewegner"));

            var authHeader = _authorization.GetHeader(uri);
            Assert.IsNotNull(authHeader);

            await HttpSend<FriendsIdsModel>(authHeader, uri);
        }

        [Test]
        public async Task UsersLookup()
        {
            var uri = new Uri(string.Format("{0}?{1}", Urls.UsersLookup, "screen_name=wadewegner,smarx"));

            var authHeader = _authorization.GetHeader(uri);
            Assert.IsNotNull(authHeader);

            await HttpSend<List<UserLookupModel>>(authHeader, uri);
        }

        [Test]
        public async Task StatusesUpdate()
        {
            const string query = "status=Test&display_coordinates=false";
            var uri = new Uri(string.Format("{0}?{1}", Urls.StatusesUpdate, query));

            var authHeader = _authorization.GetHeader(uri, HttpMethod.Post);
            Assert.IsNotNull(authHeader);

            await HttpSend<StatusesUpdateModel>(authHeader, uri, HttpMethod.Post);
        }

        private static async Task HttpSend<T>(string authHeader, Uri uri, HttpMethod httpMethod = null)
        {
            if (httpMethod == null)
                httpMethod = HttpMethod.Get;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authHeader);

            var request = new HttpRequestMessage
            {
                RequestUri = uri,
                Method = httpMethod
            };

            var responseMessage = await _httpClient.SendAsync(request).ConfigureAwait(false);

            Assert.IsNotNull(responseMessage);
            Assert.IsTrue(responseMessage.IsSuccessStatusCode, responseMessage.ToString());

            var response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            var jToken = JToken.Parse(response);
            if (jToken.Type == JTokenType.Array)
            {
                var jArray = JArray.Parse(response);

                var responseObject = JsonConvert.DeserializeObject<T>(jArray.ToString());
                Assert.IsNotNull(responseObject);
            }
            else
            {
                var jObject = JObject.Parse(response);

                var responseObject = JsonConvert.DeserializeObject<T>(jObject.ToString());
                Assert.IsNotNull(responseObject);
            }
        }
    }
}
