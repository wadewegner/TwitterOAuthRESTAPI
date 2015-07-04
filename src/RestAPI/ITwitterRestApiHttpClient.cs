using System.Net.Http;
using System.Threading.Tasks;

namespace TwitterOAuth.RestAPI
{
    public interface ITwitterRestApiHttpClient
    {
        Task<T> GetAsync<T>(string resourceUri, dynamic parameters);

        Task<T> GetAsync<T>(string resourceUri);

        Task<HttpResponseMessage> GetHttpResponseMessageAsync(string resourceUri, dynamic parameters);

        Task<HttpResponseMessage> GetHttpResponseMessageAsync(string resourceUri);
    }
}