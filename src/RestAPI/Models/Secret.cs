using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterOAuth.RestAPI.Models
{
    public class Secret
    {


        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

    }
}
