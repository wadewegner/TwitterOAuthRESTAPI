using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterOAuth.RestAPI
{
    public interface ITwitterRestApiHttpClient
    {
        Task<T> GetAsync<T>(string resourceUri, dynamic parameters);

        Task<T> GetAsync<T>(string resourceUri);
    }
}
