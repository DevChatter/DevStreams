using DevChatter.DevStreams.Core.Settings;
using DevChatter.DevStreams.Core.Twitch;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
            var channelIdsQueryFormat = String.Join("&user_id=", twitchIds);

            var url = $"{_twitchSettings.BaseApiUrl}/streams?user_id={channelIdsQueryFormat}";
            var jsonResult = await Get(url);

            var result = JsonConvert.DeserializeObject<StreamResult>(jsonResult);

            var liveChannels = result.Data.ToList();

            List<ChannelLiveState> returnStat = twitchIds
                .Select(twitchId => new ChannelLiveState
                {
                    TwitchId = twitchId,
                    IsLive = liveChannels.Any(x => x.User_id == twitchId),
                    StartedAt = result.Data.Where(x => x.User_id == twitchId).Select(x => x.Started_at.ToUniversalTime()).DefaultIfEmpty().First(),
                    ViewerCount = result.Data.Where(x => x.User_id == twitchId).Select(x => x.Viewer_count).DefaultIfEmpty().First()

                })
                .ToList();
            return returnStat;
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
