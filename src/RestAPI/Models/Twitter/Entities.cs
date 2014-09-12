using System.Collections.Generic;

namespace TwitterOAuth.RestAPI.Models.Twitter
{
    public class Entities
    {
        public List<object> urls { get; set; }
        public List<Hashtag> hashtags { get; set; }
        public List<UserMention> user_mentions { get; set; }
        public Url url { get; set; }
        public Description description { get; set; }
        public List<object> symbols { get; set; }
    }
}