using Dapper;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Dapper.Services
{
    public class ChannelSearchService : IChannelSearchService
    {
        private readonly DatabaseSettings _dbSettings;

        public ChannelSearchService(IOptions<DatabaseSettings> databaseSettings)
        {
            _dbSettings = databaseSettings.Value;
        }

        public async Task<Channel> GetChannelSoundex(string standardizedChannelName)
        {
            var sql = @"SELECT TOP 1 * FROM Channels WHERE SOUNDEX(Name) = SOUNDEX(@standardizedChannelName) 
                        ORDER BY DIFFERENCE(Name, @standardizedChannelName) DESC";
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                using (var multi = await connection.QueryMultipleAsync(sql, new { standardizedChannelName }))
                {
                    return (await multi.ReadAsync<Channel>()).SingleOrDefault();
                }
            }
        }
    }
}