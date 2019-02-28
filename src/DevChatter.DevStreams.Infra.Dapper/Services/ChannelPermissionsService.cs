using System.Data;
using System.Data.SqlClient;
using Dapper;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Options;

namespace DevChatter.DevStreams.Infra.Dapper.Services
{
    public class ChannelPermissionsService : IChannelPermissionsService
    {
        private readonly DatabaseSettings _dbSettings;

        public ChannelPermissionsService(IOptions<DatabaseSettings> databaseSettings)
        {
            _dbSettings = databaseSettings.Value;
        }

        public bool CanAccessChannel(string userId, int channelId)
        {
            using (var conn = new SqlConnection(_dbSettings.DefaultConnection))
            {
                const string sql = "WHERE ChannelId = @channelId and UserId = @userId";
                return conn.RecordCount<ChannelPermission>(sql, new { userId, channelId }) > 0;
            }
        }
    }
}