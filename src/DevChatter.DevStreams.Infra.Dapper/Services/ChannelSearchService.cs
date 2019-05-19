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

        /// <summary>
        /// Get Channels By Tag IDs. Channel must have at least one of the tags.
        /// </summary>
        /// <param name="tagIds">Array of tag ids.</param>
        /// <returns>List of Channels having at least one of the passed TagIds, order by most matches.</returns>
        public async Task<List<Channel>> GetChannelsByTagMatches(params int[] tagIds)
        {
            var sql = @"SELECT c.[Id], c.[Name], c.[Uri], c.[CountryCode], c.[TimeZoneId]
                            FROM Channels c, ChannelTags t
                            WHERE c.Id = t.ChannelId
                            AND t.TagId IN @tagIds
                            GROUP BY c.[Id], c.[Name], c.[Uri], c.[CountryCode], c.[TimeZoneId]
                            ORDER BY COUNT(*) DESC";
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                using (var multi = await connection.QueryMultipleAsync(sql, new { tagIds }))
                {
                    return (await multi.ReadAsync<Channel>()).ToList();
                }
            }
        }
    }
}