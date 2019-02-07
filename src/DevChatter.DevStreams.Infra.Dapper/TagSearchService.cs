using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using Dapper;

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
                List<Tag> output = (await connection.QueryAsync<Tag>(sql,
                    new { Search = $"%{filter}%" })).ToList();
                return output;

            }
        }
    }
}