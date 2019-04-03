using DevChatter.DevStreams.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Services
{
    public interface ITagService
    {
        Task<ILookup<int, Tag>> GetChannelTagsLookup(IEnumerable<int> channelIds);
    }
}
