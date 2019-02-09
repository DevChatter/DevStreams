using Dapper.Contrib.Extensions;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
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

        public async Task<T> Get<T>(int id) where T : class
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.GetAsync<T>(id);
            }
        }

        public async Task<bool> Update<T>(T model) where T : class
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return await connection.UpdateAsync(model);
            }
        }

    }
}