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
            : this(new Authorization(new SecretModel() { ApiKey = apiKey, ApiSecret = apiSecret, AccessToken = accessToken, AccessTokenSecret = accessTokenSecret }))
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
            var resourceUriWithQueryString = UriHelper.GetResourceUriWithQueryString(resourceUri, parameters);

            return await this.GetAsync<T>(resourceUriWithQueryString);
        }

        public async Task<T> GetAsync<T>(string resourceUri)
        {
            var responseMessage = await this.GetHttpResponseMessageAsync(resourceUri);
            var responseContent = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                return JsonHelper.ParseResponseContent<T>(responseContent);
            }

            throw new ApplicationException(string.Format("Received Non Success Status Code. {0} {1}", responseMessage.StatusCode, responseContent));
        }

        public async Task<HttpResponseMessage> GetHttpResponseMessageAsync(string resourceUri, dynamic parameters)
        {
            var resourceUriWithQueryString = UriHelper.GetResourceUriWithQueryString(resourceUri, parameters);

            return await this.GetHttpResponseMessageAsync(resourceUriWithQueryString);
        }

        public async Task<HttpResponseMessage> GetHttpResponseMessageAsync(string resourceUri)
        {
            var requestUri = new Uri(resourceUri);

            var signedHeader = this.Authorization.GetHeader(requestUri, HttpMethod.Get);

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = requestUri,
                Method = HttpMethod.Get
            };
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("OAuth", signedHeader);

            return await this.HttpClient.SendAsync(requestMessage).ConfigureAwait(false);
        }
    }
}