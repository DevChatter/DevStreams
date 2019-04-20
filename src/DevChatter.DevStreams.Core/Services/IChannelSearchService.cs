using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Core.Services
{
    public interface IChannelSearchService
    {
        List<Channel> Find();
        Task<Channel> GetChannelSoundex(string standardizedChannelName);
    }
}