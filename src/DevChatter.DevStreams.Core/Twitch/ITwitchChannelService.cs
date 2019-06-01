using DevChatter.DevStreams.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Twitch
{
    public interface ITwitchChannelService
    {
        Task<TwitchChannel> GetChannelInfo(string channelName);
        Task<List<TwitchChannel>> GetChannelsInfo(params string[] channelNames);
    }
}
