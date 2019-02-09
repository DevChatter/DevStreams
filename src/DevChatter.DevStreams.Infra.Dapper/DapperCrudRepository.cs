using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Dapper
{
    public class DapperCrudRepository : ICrudRepository
    {
        private readonly DatabaseSettings _dbSettings;

        public DapperCrudRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _dbSettings = databaseSettings.Value;
        }

        public async Task<T> Insert<T>(T model) where T : DataEntity
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                int id = await connection.InsertAsync<T>(model);
                return await connection.GetAsync<T>(id);
            }
        }

        public async Task<T> Get<T>(int id) where T : DataEntity
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.GetAsync<T>(id);
            }
        }

        public async Task<List<T>> GetAll<T>() where T : DataEntity
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return (await connection.GetAllAsync<T>()).ToList();
            }
        }

        public async Task<bool> Update<T>(T model) where T : DataEntity
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.UpdateAsync(model);
            }
        }

        public async Task<bool> Delete<T>(int id) where T : DataEntity, new()
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.DeleteAsync(new T { Id = id });
            }
        }

        public async Task<bool> Delete<T>(T model) where T : DataEntity
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.DeleteAsync(model);
            }
        }
    }
}