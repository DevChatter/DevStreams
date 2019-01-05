using System.Collections.Generic;
using NodaTime;

namespace DevChatter.DevStreams.Core.Model
{
    /// <summary>
    /// A regularly scheduled time for a stream to happen.
    /// Example: DevChatter streams on Mondays at 2 PM
    /// </summary>
    public class ScheduledStream : DataEntity
    {
        public List<StreamSession> Sessions { get; set; }
            = new List<StreamSession>();
        public Channel Channel { get; set; }
        public IsoDayOfWeek DayOfWeek { get; set; }
        public LocalTime LocalStartTime { get; set; }
        public LocalTime LocalEndTime { get; set; }
    }
}