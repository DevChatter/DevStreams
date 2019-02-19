using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Core.Services
{
    public interface IScheduledStreamService
    {
        Task<List<ScheduledStream>> GetChannelSchedule(int channelId);
        Task<int?> AddScheduledStreamToChannel(ScheduledStream stream);
        Task<int> Delete(int id);
        Task<int> Update(ScheduledStream stream);
    }
}