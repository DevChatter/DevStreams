using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Twitch;

namespace DevChatter.DevStreams.Web.Data.ViewModel.LiveChannels
{
    public static class LiveChannelsMappingExtensions
    {
        public static LiveChannelViewModel ToViewModel(this ChannelLiveState src, Channel channel)
        {
            var viewModel = new LiveChannelViewModel
            {
                Id = channel.Id,
                ChannelName = channel.Name,
                Uri = channel.Uri,
                StartedAt = src.StartedAt,
                ViewerCount = src.ViewerCount,
            };

            return viewModel;
        }
    }
}