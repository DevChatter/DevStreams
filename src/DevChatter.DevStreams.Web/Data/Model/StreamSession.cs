using NodaTime;

namespace DevChatter.DevStreams.Web.Data.Model
{
    public class StreamSession
    {
        public LocalDateTime LocalDateTime { get; set; }
        public string TimeZoneId { get; set; }
        public Instant UtcInstant { get; set; }
        public string TzdbVersionId { get; set; }
    }
}