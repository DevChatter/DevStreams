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
    public class TwitchChannelService : ITwitchChannelService
    {
        private readonly DatabaseSettings _dbSettings;

        public TwitchChannelService(IOptions<DatabaseSettings> databaseSettings)
        {
            _dbSettings = databaseSettings.Value;
        }

        public async Task<IDictionary<int, TwitchChannel>> GetTwitchChannel(IEnumerable<int> channelIds)
        {
            string sql = $"SELECT * FROM TwitchChannels WHERE ChannelId IN @ChannelIds";

            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var twitchChannels = (await connection.QueryAsync<TwitchChannel>(sql,
                    new { ChannelIds = channelIds }));

                return twitchChannels.ToDictionary(x => x.ChannelId);
            }
        }
    }
}
