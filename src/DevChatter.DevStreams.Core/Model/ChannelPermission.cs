using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevChatter.DevStreams.Core.Model
{
    [Table("ChannelPermissions")]
    public class ChannelPermission
    {
        public int ChannelId { get; set; }
        public string UserId { get; set; }
        public ChannelRole ChannelRole { get; set; }
    }
}