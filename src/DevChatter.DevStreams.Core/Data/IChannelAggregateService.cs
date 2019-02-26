using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Core.Data
{
    public interface IChannelAggregateService
    {
        List<Channel> GetAll();
        List<Channel> GetAll(string userId);
        Channel GetAggregate(int id);
        Task<int?> Create(Channel model, string userId);
        Task<int> Update(Channel model);
        Task<int> Delete(int id);
    }
}