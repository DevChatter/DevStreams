using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
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
        /// 
        /// </summary>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        public async Task<List<Channel>> GetChannelsByTagIds(params int[] tagIds)
        {
            const string sql = @"
            SELECT DISTINCT c.*
            FROM [Channels] c 
            INNER JOIN [ChannelTags] ct ON ct.ChannelId = c.Id
            WHERE ct.TagId in @tagIds";

            return await QueryAsync<Channel>(sql, new { tagIds });
        }
    }
}