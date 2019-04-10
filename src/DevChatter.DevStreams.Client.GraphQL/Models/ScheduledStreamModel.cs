using NodaTime;

namespace DevChatter.DevStreams.Client.GraphQL.Models
{
    public class ScheduledStreamModel
    {
        public int Id { get; set; }
        public string ChannelId { get; set; }
        public string TimeZoneId { get; set; }
        public IsoDayOfWeek DayOfWeek { get; set; }
        public string LocalStartTime { get; set; }
        public string LocalEndTime { get; set; }
    }
}
