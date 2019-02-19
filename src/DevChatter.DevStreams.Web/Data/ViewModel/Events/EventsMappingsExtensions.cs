using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Web.Data.ViewModel.Events
{
    public static class EventsMappingsExtensions
    {
        public static EventViewModel ToViewModel(this StreamSession src, Channel channel)
        {
            return new EventViewModel
            {
                Id = src.Id,
                ScheduledStreamId = src.ScheduledStreamId,
                ChannelId = src.ChannelId,
                ChannelName = channel.Name,
                Uri = channel.Uri,
                UtcStartTime = src.UtcStartTime.ToDateTimeUtc(),
                UtcEndTime = src.UtcEndTime.ToDateTimeUtc(),
            };
        }
    }
}