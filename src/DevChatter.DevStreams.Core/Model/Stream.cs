using System;
using System.Collections.Generic;

namespace DevChatter.DevStreams.Core.Model
{
    /// <summary>
    /// Information about a stream. Example: DevChatter
    /// </summary>
    public class Stream
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public List<StreamTime> StreamTimes { get; set; }
        public string TimeZoneId { get; set; }
    }
}