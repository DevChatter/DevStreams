using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Core.Services
{
    public interface IScheduledStreamService
    {
        Task<List<ScheduledStream>> GetChannelSchedule(int channelId);
        Task<ILookup<int, ScheduledStream>> GetChannelScheduleLookup(IEnumerable<int> channelIds);
        Task<int?> AddScheduledStreamToChannel(ScheduledStream stream);
        Task<int> Delete(int id);
        Task<int> Update(ScheduledStream stream);
    }
}