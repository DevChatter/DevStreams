using NodaTime;

namespace DevChatter.DevStreams.Core.Model
{
    public class StreamSession
    {
        public LocalDateTime LocalStartDateTime { get; set; }
        public LocalDateTime LocalEndDateTime { get; set; }
        public string TimeZoneId { get; set; }
        public Instant UtcStartTime { get; set; }
        public Instant UtcEndTime { get; set; }
        public string TzdbVersionId { get; set; }
    }
}