using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.TwitchHelper
{
    public interface ITwitchService
    {
        Task<List<string>> GetChannelIds();
        Task<List<string>> GetLiveChannels();
    }
}
