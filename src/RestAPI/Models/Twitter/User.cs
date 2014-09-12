namespace TwitterOAuth.RestAPI.Models.Twitter
{
    public class User
    {
        public string profile_sidebar_fill_color { get; set; }
        public string profile_sidebar_border_color { get; set; }
        public bool profile_background_tile { get; set; }
        public string name { get; set; }
        public string profile_image_url { get; set; }
        public string created_at { get; set; }
        public string location { get; set; }
        public object follow_request_sent { get; set; }
        public string profile_link_color { get; set; }
        public bool is_translator { get; set; }
        public string id_str { get; set; }
        public Entities entities { get; set; }
        public bool default_profile { get; set; }
        public bool contributors_enabled { get; set; }
        public int favourites_count { get; set; }
        public string url { get; set; }
        public string profile_image_url_https { get; set; }
        public int? utc_offset { get; set; }
        public double id { get; set; }
        public bool profile_use_background_image { get; set; }
        public int listed_count { get; set; }
        public string profile_text_color { get; set; }
        public string lang { get; set; }
        public int followers_count { get; set; }
        public bool @protected { get; set; }
        public object notifications { get; set; }
        public string profile_background_image_url_https { get; set; }
        public string profile_background_color { get; set; }
        public bool verified { get; set; }
        public bool geo_enabled { get; set; }
        public string time_zone { get; set; }
        public string description { get; set; }
        public bool default_profile_image { get; set; }
        public string profile_background_image_url { get; set; }
        public int statuses_count { get; set; }
        public int friends_count { get; set; }
        public object following { get; set; }
        public bool show_all_inline_media { get; set; }
        public string screen_name { get; set; }
        public bool is_translation_enabled { get; set; }
        public string profile_banner_url { get; set; }
    }
}