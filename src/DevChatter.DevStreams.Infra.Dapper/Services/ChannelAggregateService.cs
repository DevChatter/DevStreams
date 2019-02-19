using Dapper;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Dapper.Services
{
    public class ChannelAggregateService : IChannelAggregateService
    {
        private readonly DatabaseSettings _dbSettings;

        public ChannelAggregateService(IOptions<DatabaseSettings> databaseSettings)
        {
            _dbSettings = databaseSettings.Value;
        }

        public Channel GetAggregate(int id)
        {
            const string sql =
                @"SELECT top 1 c.*, ss.Id, t.*
                FROM [Channels] c
                INNER JOIN [ScheduledStream] ss on ss.ChannelId = c.Id
                INNER JOIN [ChannelTag] ct on ct.ChannelId = c.Id
                INNER JOIN [Tags] t on t.Id = ct.TagId
                WHERE c.Id = @id";

            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                Channel channels = connection.Query<Channel, int, Tag, Channel>(
                        sql, MapFullChannel, new {id}, splitOn: "ChannelId,TagId")
                    .GroupBy(channel => channel.Id)
                    .Select(ChannelGroupSelector)
                    .SingleOrDefault();
                return channels;
            }
        }

        public async Task<int?> Create(Channel model)
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                int? id = await connection.InsertAsync(model);

                var channelTags = model.Tags
                    .Select(tag => new ChannelTag {ChannelId = model.Id, TagId = tag.Id});

                await Task.WhenAll(channelTags.Select(ct => connection.InsertAsync(ct)));

                return id;
            }
        }

        public async Task<int> Update(Channel model)
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                int rows = await connection.UpdateAsync(model);

                var channelTags = model.Tags
                    .Select(tag => new ChannelTag { ChannelId = model.Id, TagId = tag.Id });

                await connection.DeleteListAsync<ChannelTag>(new {ChannelId = model.Id});

                await Task.WhenAll(channelTags.Select(ct => connection.InsertAsync(ct)));

                return rows;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var conditions = new { ChannelId = id };
                await connection.DeleteListAsync<ChannelTag>(conditions);
                await connection.DeleteListAsync<StreamSession>(conditions);
                await connection.DeleteListAsync<ScheduledStream>(conditions);

                return await connection.DeleteAsync<Channel>(id);
            }
        }

        private Channel ChannelGroupSelector(IGrouping<int, Channel> grp)
        {
            Channel channel = grp.First();
            channel.Tags = grp.SelectMany(each => each.Tags).ToList();
            return channel;
        }

        private Channel MapFullChannel(Channel channel, int scheduledStreamId, Tag tag)
        {
            channel.ScheduledStreamIds.Add(scheduledStreamId);
            channel.Tags.Add(tag);
            return channel;
        }
    }
}