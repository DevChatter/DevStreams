using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Twitch
{
    public interface ITwitchService
    {
        Task<List<string>> GetChannelIds(List<string> channelNames);
        Task<List<string>> GetLiveChannels(List<string> channelNames);
        Task<bool> IsLive(string twitchId);
    }
}
