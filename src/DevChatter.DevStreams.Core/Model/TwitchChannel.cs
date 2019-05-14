namespace DevChatter.DevStreams.Core.Model
{
    public class TwitchChannel
    {
        public int ChannelId { get; set; }
        public string TwitchId { get; set; }
        public string TwitchName { get; set; }
        public string Description { get; set; }
        public bool IsAffiliate { get; set; }
        public bool IsPartner { get; set; }
        public string ImageUrl { get; set; }
    }
}