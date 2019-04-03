using DevChatter.DevStreams.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Services
{
    public interface ITwitchChannelService
    {
        Task<IDictionary<int, TwitchChannel>> GetTwitchChannel(IEnumerable<int> channelIds);
    }
}
