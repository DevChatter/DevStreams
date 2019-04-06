using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Twitch
{
    public interface ITwitchService
    {
        Task<List<ChannelLiveState>> GetChannelLiveStates(List<string> twitchIds);
        Task<bool> IsLive(string twitchId);
    }
}
