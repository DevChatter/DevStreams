using DevChatter.DevStreams.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Services
{
    public interface IChannelSearchService
    {
        Task<Channel> GetChannelSoundex(string standardizedChannelName);
        Task<List<Channel>> GetChannelsByTagMatches(params int[] tagIds);
    }
}