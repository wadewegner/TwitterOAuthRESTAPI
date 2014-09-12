using System.Collections.Generic;
using TwitterOAuth.RestAPI.Models.Twitter;

namespace TwitterOAuth.RestAPI.Models
{
    public class SearchTweetsModel
    {
        public List<Status> statuses { get; set; }
        public SearchMetadata search_metadata { get; set; }
    }
}
