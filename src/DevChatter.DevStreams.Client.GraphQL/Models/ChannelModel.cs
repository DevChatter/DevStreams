using System.Collections.Generic;

namespace DevChatter.DevStreams.Client.GraphQL.Models
{
    public class ChannelModel
    {
        // TODO: fill out the rest of the properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string CountryCode { get; set; }
        public string TimeZoneId { get; set; }
        public TwitchModel Twitch { get; set; } = new TwitchModel();
        public StreamSessionModel NextStream { get; set; }
        public List<StreamSessionModel> FutureStreams { get; set; } = new List<StreamSessionModel>();
        public List<ScheduledStreamModel> Schedule { get; set; } = new List<ScheduledStreamModel>();
        public List<TagModel> Tags { get; set; } = new List<TagModel>();
    }
}
