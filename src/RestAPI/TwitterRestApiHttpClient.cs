using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TwitterOAuth.RestAPI.Models;
using TwitterOAuth.RestAPI.Resources;

namespace TwitterOAuth.RestAPI
{
    public class TwitterRestApiHttpClient : ITwitterRestApiHttpClient
    {
        private Authorization Authorization { get; set; }

        private HttpClient HttpClient { get; set; }

        public TwitterRestApiHttpClient(string apiKey, string apiSecret, string accessToken, string accessTokenSecret)
            : this(new Authorization(new SecretModel() { ApiKey = apiKey, ApiSecret = apiSecret, AccessToken = accessToken, AccessTokenSecret = accessTokenSecret}))
        {
            // no-op
        }

        public TwitterRestApiHttpClient(Authorization authorization)
            : this(authorization, new HttpClient())
        {
            // no-op
        }

        public TwitterRestApiHttpClient(Authorization authorization, HttpClient httpClient)
        {
            this.Authorization = authorization;
            this.HttpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string resourceUri, dynamic parameters)
        {
            var queryString = UriHelper.ConvertDynamicToQueryStringParameters(parameters);

            var resourceUriWithQueryString = string.Format("{0}?{1}", resourceUri, queryString);

            return await this.GetAsync<T>(resourceUriWithQueryString);
        }

        public async Task<T> GetAsync<T>(string resourceUri)
        {
            var twitterResponse = await this.GetTwitterResponse(resourceUri);

            if (twitterResponse.HttpResponseMessage.IsSuccessStatusCode)
            {
                return JsonHelper.ParseResponseContent<T>(twitterResponse.Content);
            }

            throw new ApplicationException(string.Format("Received Non Success Status Code. {0} {1}", twitterResponse.HttpResponseMessage.StatusCode, twitterResponse.Content));
        }

        private async Task<TwitterResponse> GetTwitterResponse(string resourceUri)
        {
            var requestUri = new Uri(resourceUri);

            var signedHeader = this.Authorization.GetHeader(requestUri, HttpMethod.Get);
            
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", signedHeader);

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = requestUri,
                Method = HttpMethod.Get
            };

            var responseMessage = await this.HttpClient.SendAsync(requestMessage).ConfigureAwait(false);
            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new TwitterResponse() { HttpResponseMessage = responseMessage, Content = responseContent };
        }
        
        private class TwitterResponse
        {
            public HttpResponseMessage HttpResponseMessage { get; set; }
            public string Content { get; set; }
        }
    }
}