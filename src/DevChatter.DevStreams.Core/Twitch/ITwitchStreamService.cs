using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Twitch
{
    public interface ITwitchStreamService
    {
        Task<List<ChannelLiveState>> GetChannelLiveStates(List<string> twitchIds);
        Task<ChannelLiveState> IsLive(string twitchId);
    }
}
