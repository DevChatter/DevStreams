using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Core.Services
{
    public interface IScheduledStreamService
    {
        void AddScheduledStreamToChannel(Channel channel, ScheduledStream stream);
    }
}