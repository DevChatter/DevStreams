namespace DevChatter.DevStreams.Core.Services
{
    public interface IChannelPermissionsService
    {
        bool CanAccessChannel(string userId, int channelId);
    }
}