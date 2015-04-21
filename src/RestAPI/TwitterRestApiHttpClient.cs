using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterOAuth.RestAPI
{
    public class TwitterRestApiHttpClient : ITwitterRestApiHttpClient
    {
        private Authorization Authorization { get; set; }

        public TwitterRestApiHttpClient(Authorization authorization)
        {
            this.Authorization = authorization;
        }

        public async Task<T> GetAsync<T>(string resourceUri, dynamic parameters)
        {
            throw new NotImplementedException();
        }
    }
}