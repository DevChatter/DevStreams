using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Core.Data
{
    public interface IChannelRepository : ICrudRepository
    {
        Task<List<Channel>> GetChannelsByTagIds(params int[] tagIds);
    }
}