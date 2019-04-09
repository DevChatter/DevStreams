using System;
using Dapper;
using DevChatter.DevStreams.Core.Data;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
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

        public List<Channel> GetAll()
        {
            const string channelSql = "SELECT * FROM Channels";
            const string extraSql =
                @"SELECT * FROM ScheduledStreams WHERE ChannelId = @id;
                  SELECT t.* FROM ChannelTags ct INNER JOIN Tags t ON t.Id = ct.TagId WHERE ct.ChannelId = @id";

            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var channels = connection.Query<Channel>(channelSql).ToList();

                foreach (var channel in channels)
                {
                    using (var multi = connection.QueryMultiple(extraSql, new {channel.Id}))
                    {
                        channel.ScheduledStreams = multi.Read<ScheduledStream>().ToList();
                        channel.Tags = multi.Read<Tag>().ToList();
                    }
                }

                return channels;
            }
        }

        public List<Channel> GetAll(string userId)
        {
            const string channelSql = "SELECT * FROM Channels c INNER JOIN ChannelPermissions cp on cp.ChannelId = c.Id WHERE cp.UserId = @userId";
            const string extraSql =
                @"SELECT * FROM ScheduledStreams WHERE ChannelId = @id;
                  SELECT t.* FROM ChannelTags ct INNER JOIN Tags t ON t.Id = ct.TagId WHERE ct.ChannelId = @id";

            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var channels = connection
                    .Query<Channel>(channelSql, new { userId })
                    .ToList();

                foreach (var channel in channels)
                {
                    using (var multi = connection.QueryMultiple(extraSql, new { channel.Id }))
                    {
                        channel.ScheduledStreams = multi.Read<ScheduledStream>().ToList();
                        channel.Tags = multi.Read<Tag>().ToList();
                    }
                }

                return channels;
            }
        }

        public Channel GetAggregate(int id)
        {
            const string sql =
                @"SELECT * FROM Channels WHERE Id = @id;
                  SELECT * FROM TwitchChannels WHERE ChannelId = @id;
                  SELECT * FROM ScheduledStreams WHERE ChannelId = @id;
                  SELECT t.* FROM ChannelTags ct INNER JOIN Tags t ON t.Id = ct.TagId WHERE ct.ChannelId = @id";

            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                using (var multi = connection.QueryMultiple(sql, new {id}))
                {
                    var channel = multi.Read<Channel>().First();
                    channel.Twitch = multi.Read<TwitchChannel>().FirstOrDefault();
                    channel.ScheduledStreams = multi.Read<ScheduledStream>().ToList();
                    channel.Tags = multi.Read<Tag>().ToList();
                    return channel;
                }
            }
        }

        public async Task<int?> Create(Channel model, string userId)
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                int? id = await connection.InsertAsync(model);

                var channelTags = model.Tags
                    .Select(tag => new ChannelTag {ChannelId = model.Id, TagId = tag.Id});

                var channelPermission = new ChannelPermission
                {
                    ChannelId = id.Value,
                    UserId = userId,
                    ChannelRole = ChannelRole.Owner
                };
                const string insertPermissionsSql = "INSERT INTO [ChannelPermissions] (ChannelId, UserId, ChannelRole) VALUES (@ChannelId, @UserId, @ChannelRole)";
                await connection.ExecuteAsync(insertPermissionsSql, channelPermission);

                model.Twitch.ChannelId = id.Value;
                const string insertTwitchChannelSql = "INSERT INTO [TwitchChannels] (ChannelId, TwitchId, TwitchName, IsAffiliate, IsPartner) VALUES (@ChannelId, @TwitchId, @TwitchName, @IsAffiliate, @IsPartner)";
                await connection.ExecuteAsync(insertTwitchChannelSql, model.Twitch);

                const string insertChannelTagSql = "INSERT INTO [ChannelTags] (ChannelId, TagId) VALUES (@ChannelId, @TagId)";
                await Task.WhenAll(channelTags.Select(ct 
                    => connection.ExecuteAsync(insertChannelTagSql, ct)));

                return id;
            }
        }

        public async Task<int> Update(Channel model)
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                int rows = await connection.UpdateAsync(model);

                var channelTags = model.Tags
                    .Select(tag => new ChannelTag {ChannelId = model.Id, TagId = tag.Id});

                await connection.DeleteListAsync<ChannelTag>(new {ChannelId = model.Id});

                await Task.WhenAll(channelTags.Select(ct => connection.InsertAsync(ct)));

                // TODO: Update the TwitchInfo on channel Edit

                return rows;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var conditions = new {ChannelId = id};
                await connection.DeleteListAsync<ChannelTag>(conditions);
                await connection.DeleteListAsync<StreamSession>(conditions);
                await connection.DeleteListAsync<ScheduledStream>(conditions);

                return await connection.DeleteAsync<Channel>(id);
            }
        }
    }
}