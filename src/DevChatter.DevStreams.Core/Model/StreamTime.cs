using NodaTime;

namespace DevChatter.DevStreams.Core.Model
{
    public class StreamTime
    {
        public string TimeZoneId { get; set; }
        public IsoDayOfWeek DayOfWeek { get; set; }
        public LocalTime LocalTime { get; set; }
    }
}