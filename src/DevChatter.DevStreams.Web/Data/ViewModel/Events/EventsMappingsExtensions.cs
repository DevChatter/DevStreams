using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Web.Data.ViewModel.Events
{
    public static class EventsMappingsExtensions
    {
        public static EventViewModel ToViewModel(this StreamSession src)
        {
            return new EventViewModel
            {
                Id = src.Id,
                ScheduledStreamId = src.ScheduledStream.Id,
                ChannelId = src.ScheduledStream.Channel.Id,
                ChannelName = src.ScheduledStream.Channel.Name,
                Uri = src.ScheduledStream.Channel.Uri,
                UtcStartTime = src.UtcStartTime.ToDateTimeUtc(),
                UtcEndTime = src.UtcEndTime.ToDateTimeUtc(),
            };
        }
    }
}