using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Data
{
    public interface ICrudRepository
    {
        Task<T> Get<T>(int id) where T : class;
        Task<bool> Update<T>(T model) where T : class;
    }
}