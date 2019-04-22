using DevChatter.DevStreams.Core.Tagging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Services
{
    public interface ITagSearchService
    {
        Task<List<TagWithCount>> FindTagsWithCount(string filter);
    }
}