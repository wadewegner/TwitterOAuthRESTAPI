using NUnit.Framework;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Threading.Tasks;
using TwitterOAuth.RestAPI;
using TwitterOAuth.RestAPI.Models;
using TwitterOAuth.RestAPI.Resources;

namespace RestAPI.Tests
{
    [TestFixture]
    public class TwitterRestApiHttpClientFunctionalTests
    {
        private SecretModel _secretModel;
        private Authorization _authorization;

        private readonly string _apiKey = ConfigurationManager.AppSettings["ApiKey"];
        private readonly string _apiSecret = ConfigurationManager.AppSettings["ApiSecret"];
        private readonly string _accessToken = ConfigurationManager.AppSettings["AccessToken"];
        private readonly string _accessTokenSecret = ConfigurationManager.AppSettings["AccessTokenSecret"];

        [TestFixtureSetUp]
        public void Init()
        {
            _secretModel = new SecretModel
            {
                ApiKey = _apiKey,
                ApiSecret = _apiSecret,
                AccessToken = _accessToken,
                AccessTokenSecret = _accessTokenSecret
            };

            _authorization = new Authorization(_secretModel);
        }

        [Test]
        public async Task GetFriendsListDynamic()
        {
            // arrange
            dynamic parameters = new ExpandoObject();
            parameters.screen_name = "twitter";

            // sut
            var client = new TwitterRestApiHttpClient(_authorization);

            // act
            dynamic response = await client.GetAsync<dynamic>(Urls.FriendsList, parameters);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.users);
        }

        [Test]
        [Ignore]
        // This test is currently failing due to an exception being thrown within NUnit. Therefore it has been ignored.
        public async Task GetFriendsListTyped()
        {
            // arrange
            dynamic parameters = new ExpandoObject();
            parameters.screen_name = "twitter";

            // sut
            var client = new TwitterRestApiHttpClient(_authorization);

            // act
            TwitterResponseFriendsList response = await client.GetAsync<TwitterResponseFriendsList>(Urls.FriendsList, parameters);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.users);
        }

        [Test]
        public async Task GetHttpResponseMessage()
        {
            // arrange
            dynamic parameters = new ExpandoObject();
            parameters.screen_name = "twitter";

            // sut
            var client = new TwitterRestApiHttpClient(_authorization);

            var responseMessage = await client.GetHttpResponseMessageAsync(Urls.FriendsList, parameters);

            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            Assert.IsNotNull(responseMessage);
            Assert.IsNotNull(responseContent);
        }

        [Test]
        public async Task GetHttpResponseMessageError()
        {
            // arrange
            dynamic parameters = new ExpandoObject();
            parameters.screen_name = "!!!";

            // sut
            var client = new TwitterRestApiHttpClient(_authorization);

            var responseMessage = await client.GetHttpResponseMessageAsync(Urls.FriendsList, parameters);

            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            Assert.IsNotNull(responseMessage);
            Assert.IsNotNull(responseContent);
        }

        private class TwitterResponseFriendsList
        {
            public string previous_cursor { get; set; }
            public string previous_cursor_str { get; set; }
            public string next_cursor { get; set; }
            public string next_cursor_str { get; set; }
            public List<TwitterUser> users { get; set; }
        }

        private class TwitterUser
        {
            public string id { get; set; }
            public string name { get; set; }
            public string screen_name { get; set; }
            // additional properties included in response are ignored
        }
    }
}