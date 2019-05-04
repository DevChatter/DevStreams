using System;
using System.Collections.Generic;

namespace DevChatter.DevStreams.Core.Twitch
{
    public class TwitchResult
    {
    }
    public class UserResultData
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Display_name { get; set; }
        public string Type { get; set; }
        public string Broadcaster_type { get; set; }
        public string Description { get; set; }
        public string Profile_image_url { get; set; }
        public string Offline_image_url { get; set; }
        public int View_count { get; set; }
    }

    public class UserResult
    {
        public List<UserResultData> Data { get; set; }
    }

    public class StreamResultData
    {
        public string Id { get; set; }
        public string User_id { get; set; }
        public string User_name { get; set; }
        public string Game_id { get; set; }
        public List<string> Community_ids { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public int Viewer_count { get; set; }
        public DateTime Started_at { get; set; }
        public string Language { get; set; }
        public string Thumbnail_url { get; set; }
        public List<string> Tag_ids { get; set; }
    }

    public class Pagination
    {
        public string Cursor { get; set; }
    }

    public class StreamResult
    {
        public List<StreamResultData> Data { get; set; }
        public Pagination Pagination { get; set; }
    }
}
