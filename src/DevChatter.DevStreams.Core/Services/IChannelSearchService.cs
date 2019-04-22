using DevChatter.DevStreams.Core.Model;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Services
{
    public interface IChannelSearchService
    {
        Task<Channel> GetChannelSoundex(string standardizedChannelName);
    }
}