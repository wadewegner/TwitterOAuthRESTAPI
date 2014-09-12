using System.Collections.Generic;

namespace TwitterOAuth.RestAPI.Models.Twitter
{
    public class UrlData
    {
        public object expanded_url { get; set; }
        public string url { get; set; }
        public List<int> indices { get; set; }
        public string display_url { get; set; }
    }
}