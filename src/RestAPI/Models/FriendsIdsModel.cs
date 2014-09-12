using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterOAuth.RestAPI.Models
{
    public class FriendsIdsModel
    {
        public List<double> ids { get; set; }
        public int next_cursor { get; set; }
        public string next_cursor_str { get; set; }
        public int previous_cursor { get; set; }
        public string previous_cursor_str { get; set; }
    }
}
