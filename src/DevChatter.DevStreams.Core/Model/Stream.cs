using System;
using System.Collections.Generic;

namespace DevChatter.DevStreams.Core.Model
{
    public class Stream
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public List<StreamTime> StreamTimes { get; set; }
        public string TimeZoneId { get; set; }
    }
}