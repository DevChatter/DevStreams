using System.Collections.Generic;

namespace DevChatter.DevStreams.Core.Model
{
    /// <summary>
    /// Information about a streaming channel. Example: DevChatter
    /// </summary>
    public class Channel : DataEntity
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public List<int> ScheduledStreamIds { get; set; } = new List<int>();
        public string CountryCode { get; set; }
        public string TimeZoneId { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}