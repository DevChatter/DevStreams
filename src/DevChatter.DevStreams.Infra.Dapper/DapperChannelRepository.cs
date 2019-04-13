using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Dapper
{
    // TODO: Switch this to composition, instead of inheritance
    public class DapperChannelRepository : DapperCrudRepository, IChannelRepository
    {
        public DapperChannelRepository(IOptions<DatabaseSettings> databaseSettings) 
            : base(databaseSettings)
        {
        }

        /// <summary>
        /// Get Channels By Tag IDs. Channel must have all tags.
        /// </summary>
        /// <param name="tagIds">Array of tag ids.</param>
        /// <returns>List of Channels having all passed TagIds.</returns>
        public async Task<List<Channel>> GetChannelsByTagIds(params int[] tagIds)
        {
            const string sql = @"
                                SELECT *
                                FROM [Channels]
                                WHERE Id in (
                                    SELECT ChannelId 
                                    FROM [ChannelTags]
                                    WHERE TagId in @tagIds
                                    GROUP BY ChannelId
                                    HAVING count(*) = @tagCount
                                )";

            var args = new { tagIds = tagIds, tagCount = tagIds.Length };
            return await QueryAsync<Channel>(sql, args);
        }
    }
}