using DevChatter.DevStreams.Core.Settings;
using DevChatter.DevStreams.Core.Twitch;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NodaTime;
using NodaTime.Serialization.JsonNet;


namespace DevChatter.DevStreams.Infra.Twitch
{
    public class TwitchStreamService : ITwitchStreamService
    {
        private readonly TwitchSettings _twitchSettings;

        public TwitchStreamService(IOptions<TwitchSettings> twitchSettings)
        {
            _twitchSettings = twitchSettings.Value;
        }

        /// <summary>
        /// Returns the subset of the channels which are currently live on Twitch.
        /// </summary>
        /// <param name="channelNames">Names of the Channels to check for live status.</param>
        /// <returns>The names of the subset of channels that are currently live.</returns>
        public async Task<List<ChannelLiveState>> GetChannelLiveStates(List<string> twitchIds)
        {
            if (!twitchIds.Any())
            {
                return new List<ChannelLiveState>(); // TODO: Replace with Guard Clause
            }
            var channelIdsQueryFormat = string.Join("&user_id=", twitchIds);

            var url = $"{_twitchSettings.BaseApiUrl}/streams?user_id={channelIdsQueryFormat}";
            string jsonResult = await Get(url);

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            
            var result = JsonConvert.DeserializeObject<StreamResult>(jsonResult, serializerSettings);

            return result.CreateChannelLiveStatesFromStreamResults(twitchIds);
        }

        public async Task<ChannelLiveState> IsLive(string twitchId)
        {
            // TODO: Have this just check cache or do a refresh based on getting *all* data.

            var url = $"{_twitchSettings.BaseApiUrl}/streams?user_id={twitchId}";
            var jsonResult = await Get(url);

            var result = JsonConvert.DeserializeObject<StreamResult>(jsonResult);

            return new ChannelLiveState { TwitchId = twitchId, IsLive = result.Data.Any() };
        }

        // TODO: Extract to composed dependency
        private async Task<string> Get(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Client-Id", _twitchSettings.ClientId);
                var result = await client.GetStringAsync(url);
                return result;
            }
        }
    }
}
