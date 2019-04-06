using DevChatter.DevStreams.Core.Settings;
using DevChatter.DevStreams.Core.Twitch;
using DevChatter.DevStreams.Core.TwitchHelper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Twitch
{
    public class TwitchService : ITwitchStreamService
    {
        private readonly TwitchSettings _twitchSettings;

        public TwitchService(IOptions<TwitchSettings> twitchSettings)
        {
            _twitchSettings = twitchSettings.Value;
        }

        /// <summary>
        /// Converts a list of Twitch channel names to a list of Twitch ChannelIds
        /// </summary>
        /// <param name="channelNames"></param>
        /// <returns>The Twitch ChannelId/UserId for each Channel.</returns>
        public async Task<List<string>> GetChannelIds(List<string> channelNames)
        {
            // TODO: Move this to new interface, following Interface Segregation.

            var channeNamesQueryFormat = String.Join("&login=", channelNames.ToArray());

            var url = $"{_twitchSettings.BaseApiUrl}/users?login={channeNamesQueryFormat}";
            var jsonResult = await Get(url);

            var result = JsonConvert.DeserializeObject<UserResult>(jsonResult);

            return result.Data.Select(x => x.Id.ToString()).ToList();
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
                return new List<ChannelLiveState>();
            }
            var channelIdsQueryFormat = String.Join("&user_id=", twitchIds);

            var url = $"{_twitchSettings.BaseApiUrl}/streams?user_id={channelIdsQueryFormat}";
            var jsonResult = await Get(url);

            var result = JsonConvert.DeserializeObject<StreamResult>(jsonResult);

            var liveChannels = result.Data.Where(x => x.Type == "live").ToList();

            return twitchIds
                .Select(twitchId => new ChannelLiveState
                {
                    TwitchId = twitchId,
                    IsLive = liveChannels.Any(x => x.User_id == twitchId)
                })
                .ToList();
        }

        public async Task<bool> IsLive(string twitchId)
        {
            // TODO: Have this just check cache or do a refresh based on getting *all* data.

            var url = $"{_twitchSettings.BaseApiUrl}/streams?user_id={twitchId}";
            var jsonResult = await Get(url);

            var result = JsonConvert.DeserializeObject<StreamResult>(jsonResult);

            return result.Data.Any();
        }

        private async Task<string> Get(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Client-Id", _twitchSettings.ClientId);
                var result =
                    await client.GetStringAsync(url);
                return result;
            }
        }
    }
}
