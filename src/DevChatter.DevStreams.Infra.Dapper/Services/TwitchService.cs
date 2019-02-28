using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Settings;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.Extensions.Options;

namespace DevChatter.DevStreams.Core.TwitchHelper
{
    public class TwitchService : ITwitchService
    {
        private readonly string clientId = ""; // register your application on the Twitch dev portal.
        private readonly string baseApiUrl = "https://api.twitch.tv/helix";

        private readonly DatabaseSettings _dbSettings;

        public TwitchService(IOptions<DatabaseSettings> databaseSettings)
        {
            _dbSettings = databaseSettings.Value;
        }

        /// <summary>
        /// Converts a list of twitch channel names to a list of ids
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetChannelIds()
        {
            var channelNames = await GetChannelNames();
            var channeNamesQueryFormat = String.Join("&login=", channelNames.ToArray());

            var url = $"{baseApiUrl}/users?login={channeNamesQueryFormat}";
            var jsonResult = await Get(url);

            var result = JsonConvert.DeserializeObject<UserResult>(jsonResult);

            return result.Data.Select(x => x.Id.ToString()).ToList();
            
        }

        /// <summary>
        /// Pull a list of live channel names 
        /// </summary>
        /// <param name="channelIds">channel Ids</param>
        /// <returns></returns>
        public async Task<List<string>> GetLiveChannels()
        {
            var channelIds = await GetChannelIds();
            var channelIdsQueryFormat = String.Join("&user_id=", channelIds.ToArray());

            var url = $"{baseApiUrl}/streams?user_id={channelIdsQueryFormat}";
            var jsonResult = await Get(url);

            var result = JsonConvert.DeserializeObject<StreamResult>(jsonResult);

            return result.Data.Select(x => x.User_name).ToList();
        }

        private async Task<string> Get(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Client-Id", clientId);
                var result =
                    await client.GetStringAsync(url);
                return result;
            }
        }

        private async Task<List<string>> GetChannelNames()
        {
            var sql = "SELECT [Name] FROM [Channels]";
            using (IDbConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
            {
                var channelNames = connection.Query<string>(sql).ToList();

                return channelNames;
            }
        }
    }
}
