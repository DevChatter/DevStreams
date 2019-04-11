using NodaTime;
using System;

namespace DevChatter.DevStreams.Client.GraphQL.Models
{
    public class StreamSessionModel
    {
        public int Id { get; set; }
        public string ChannelId { get; set; }
        public Instant UtcStartTime { get; set; }
        public Instant UtcEndTime { get; set; }
        public DateTime LocalStartTime { get; set; }
        public DateTime LocalEndTime { get; set; }
    }
}