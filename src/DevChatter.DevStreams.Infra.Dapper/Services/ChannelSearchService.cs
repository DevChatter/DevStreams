using Dapper;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DevChatter.DevStreams.Core.Services;
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

        public List<Channel> Find()
        {
            string sql = "SELECT * FROM [ChannelTags] WHERE ChannelId IN @ids";
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                // TODO: Pull all data in 1 request and/or cache a lot.
                var channels = connection.GetList<Channel>().ToList();
                var tags = connection.GetList<Tag>().ToList(); // TODO: Cache this.

                List<int> channelIds = channels.Select(c => c.Id).ToList();
                var channelTags = connection.Query<ChannelTag>(sql, 
                    new { ids = channelIds }).ToList();

                foreach (var channel in channels)
                {
                    var tagIdsForChannel = channelTags.Where(ct => ct.ChannelId == channel.Id).Select(ct => ct.TagId);
                    channel.Tags = tags.Where(tag => tagIdsForChannel.Contains(tag.Id)).ToList();
                }

                return channels;
            }
        }

        public async Task<Channel> GetChannelSoundex(string standardizedChannelName)
        {
            var sql = @"SELECT * FROM Channels WHERE SOUNDEX(Name) = SOUNDEX(@standardizedChannelName) 
                        ORDER BY DIFFERENCE(Name, @standardizedChannelName) DESC";
            using (System.Data.IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                using (var multi = await connection.QueryMultipleAsync(sql, new { standardizedChannelName }))
                {
                    return (await multi.ReadAsync<Channel>()).FirstOrDefault();
                }
            }
        }
    }
}