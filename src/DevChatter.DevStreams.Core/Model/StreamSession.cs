using System.ComponentModel.DataAnnotations.Schema;
using NodaTime;

namespace DevChatter.DevStreams.Core.Model
{
    /// <summary>
    /// A session of a stream happening on a specific
    /// date at a specific time.
    /// Example: DevChatter streamed 2018-12-13 at 15:00 UTC.
    /// </summary>
    [Table("StreamSessions")]
    public class StreamSession : DataEntity
    {
        public int ChannelId { get; set; }
        public int ScheduledStreamId { get; set; }
        public Instant UtcStartTime { get; set; }
        public Instant UtcEndTime { get; set; }
        public string TzdbVersionId { get; set; }
    }
}