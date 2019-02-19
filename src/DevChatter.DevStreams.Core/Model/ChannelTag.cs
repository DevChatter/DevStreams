using System.ComponentModel.DataAnnotations.Schema;

namespace DevChatter.DevStreams.Core.Model
{
    [Table("ChannelTags")]
    public class ChannelTag
    {
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}