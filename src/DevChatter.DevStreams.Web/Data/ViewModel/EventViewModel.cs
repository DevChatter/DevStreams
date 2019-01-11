using System;

namespace DevChatter.DevStreams.Web.Data.ViewModel
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public int ScheduledStreamId { get; set; }
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string Uri { get; set; }
        public DateTime UtcStartTime { get; set; }
        public DateTime UtcEndTime { get; set; }
    }
}