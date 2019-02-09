using Dapper;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DevChatter.DevStreams.Core.Services;

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
            const string sql = "SELECT * FROM [Channels] c INNER JOIN [ChannelTag] ct on ct.ChannelId = c.Id INNER JOIN [Tags] t on t.Id = ct.TagId";
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                return connection.Query<Channel, ChannelTag, Tag, Channel>(sql,
                    (channel, channelTag, tag) =>
                    {
                        channelTag.Tag = tag;
                        channel.Tags.Add(channelTag);
                        return channel;
                    }, splitOn:"ChannelId,TagId")
                    .GroupBy(channel => channel.Id)
                    .Select(grp =>
                    {
                        Channel channel = grp.First();
                        channel.Tags = grp.SelectMany(each => each.Tags).ToList();
                        return channel;
                    })
                    .ToList();
            }
        }
    }
}