using System;
using NodaTime;

namespace DevChatter.DevStreams.Web.Data.Model
{
    public class StreamTime
    {
        public string TimeZoneId { get; set; }
        public IsoDayOfWeek DayOfWeek { get; set; }
        public LocalTime LocalTime { get; set; }
    }
}