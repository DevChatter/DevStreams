using Dapper;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Dapper
{
    public class TagSearchService : ITagSearchService
    {
        private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=DevStreamsDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        public async Task<List<Tag>> Find(string filter)
        {
            const string sql = "SELECT * FROM [Tags] WHERE [Name] like @Search";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var args = new { Search = $"%{filter}%" };
                List<Tag> output = (await connection.QueryAsync<Tag>(sql, args)).ToList();
                return output;
            }
        }
    }
}