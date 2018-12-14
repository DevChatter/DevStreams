using System;
using System.Collections.Generic;

namespace DevChatter.DevStreams.Core.Model
{
    /// <summary>
    /// Information about a streaming channel. Example: DevChatter
    /// </summary>
    public class Channel : DataEntity
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public List<ScheduledStream> ScheduledStreams { get; set; }
        public string TimeZoneId { get; set; }
    }
}