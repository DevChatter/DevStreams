using DevChatter.DevStreams.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Data
{
    public interface ICrudRepository
    {
        Task<T> Insert<T>(T model) where T : DataEntity;
        Task<T> Get<T>(int id) where T : DataEntity;
        Task<List<T>> GetAll<T>() where T : DataEntity;
        Task<bool> Update<T>(T model) where T : DataEntity;
        Task<bool> Delete<T>(int id) where T : DataEntity, new();
        Task<bool> Delete<T>(T model) where T : DataEntity;
        Task<bool> Exists<T>(int id) where T : DataEntity;
    }
}