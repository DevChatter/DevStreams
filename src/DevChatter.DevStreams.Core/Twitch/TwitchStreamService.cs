using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Twitch
{
    public class TwitchStreamService : ITwitchStreamService
    {
        private readonly ITwitchApiClient _twitchApiClient;

        public TwitchStreamService(ITwitchApiClient twitchApiClient)
        {
            _twitchApiClient = twitchApiClient;
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

            var url = $"/streams?user_id={channelIdsQueryFormat}";
            string jsonResult = await _twitchApiClient.GetJsonData(url);

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            
            var result = JsonConvert.DeserializeObject<StreamResult>(jsonResult, serializerSettings);

            return result.CreateChannelLiveStatesFromStreamResults(twitchIds);
        }

        public async Task<ChannelLiveState> IsLive(string twitchId)
        {
            var url = $"/streams?user_id={twitchId}";
            var jsonResult = await _twitchApiClient.GetJsonData(url);

            var result = JsonConvert.DeserializeObject<StreamResult>(jsonResult);

            return new ChannelLiveState { TwitchId = twitchId, IsLive = result.Data.Any() };
        }
    }
}
