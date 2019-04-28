using System;

namespace DevChatter.DevStreams.Core.Twitch
{
    public class ChannelLiveState
    {
        public string TwitchId { get; set; }
        public bool IsLive { get; set; }
        public DateTime startedAt { get; set; }
        public int viewerCount { get; set; }

    }
}
