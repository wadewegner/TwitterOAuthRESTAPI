namespace TwitterOAuth.RestAPI.Models.Twitter
{
    public class SearchMetadata
    {
        public long max_id { get; set; }
        public long since_id { get; set; }
        public string refresh_url { get; set; }
        public string next_results { get; set; }
        public int count { get; set; }
        public double completed_in { get; set; }
        public string since_id_str { get; set; }
        public string query { get; set; }
        public string max_id_str { get; set; }
    }
}