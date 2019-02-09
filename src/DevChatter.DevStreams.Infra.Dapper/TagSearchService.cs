using Dapper;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Dapper
{
    public class TagSearchService : ITagSearchService
    {
        private readonly DatabaseSettings _dbSettings;

        public TagSearchService(IOptions<DatabaseSettings> databaseSettings)
        {
            _dbSettings = databaseSettings.Value;
        }


        public async Task<List<Tag>> Find(string filter)
        {
            const string sql = "SELECT * FROM [Tags] WHERE [Name] like @Search";
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var args = new { Search = $"%{filter}%" };
                List<Tag> output = (await connection.QueryAsync<Tag>(sql, args)).ToList();
                return output;
            }
        }
    }
}