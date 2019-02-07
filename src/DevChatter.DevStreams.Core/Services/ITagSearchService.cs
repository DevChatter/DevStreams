using DevChatter.DevStreams.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Services
{
    public interface ITagSearchService
    {
        Task<List<Tag>> Find(string filter);
    }
}