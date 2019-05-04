using NodaTime;

namespace DevChatter.DevStreams.Web.Data.ViewModel.LiveChannels
{
    public class LiveChannelViewModel
    {
        public string ChannelName { get; set; }
        public string Uri { get; set; }
        public Instant StartedAt { get; set; }
        public Duration TimeOnline => SystemClock.Instance.GetCurrentInstant() - StartedAt;
        public int ViewerCount { get; set; }

    }
}