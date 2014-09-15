using System.Collections.Generic;

namespace TwitterOAuth.RestAPI.Models
{
    public class FollowersIdsModel
    {
        public List<double> ids { get; set; }
        public int next_cursor { get; set; }
        public string next_cursor_str { get; set; }
        public int previous_cursor { get; set; }
        public string previous_cursor_str { get; set; }
    }
}
