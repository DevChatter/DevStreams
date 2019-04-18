namespace DevChatter.DevStreams.Core.Model
{
    public class TwitchChannel
    {
        public int ChannelId { get; set; }
        public string TwitchId { get; set; }
        public string TwitchName { get; set; }
        public bool IsAffiliate { get; set; }
        public bool IsPartner { get; set; }

		// DAR added 18/0/2019 -- see issue https://github.com/DevChatter/DevStreams/issues/66#issue-434758220
		public string ImageUrl { get; set; }


	}
}