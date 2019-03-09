using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Services
{
    public interface ITwitchService
    {
        Task<List<string>> GetChannelIds(List<string> channelNames);
        Task<List<string>> GetLiveChannels(List<string> channelNames);
    }
}
