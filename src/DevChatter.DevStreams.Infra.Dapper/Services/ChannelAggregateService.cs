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
                @"SELECT * FROM Channels WHERE Id = @id;
                  SELECT Id FROM ScheduledStreams WHERE ChannelId = @id;
                  SELECT * FROM ChannelTags ct INNER JOIN Tags t ON t.Id = ct.TagId WHERE ct.ChannelId = @id";

            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                using (var multi = connection.QueryMultiple(sql, new {id}))
                {
                    var channel = multi.Read<Channel>().First();
                    channel.ScheduledStreamIds = multi.Read<int>().ToList();
                    channel.Tags = multi.Read<Tag>().ToList();
                    return channel;
                }
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
    }
}