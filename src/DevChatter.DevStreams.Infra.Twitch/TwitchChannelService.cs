using System;
using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Settings;
using DevChatter.DevStreams.Core.Twitch;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Twitch
{
    public class TwitchChannelService : ITwitchChannelService
    {
        private readonly TwitchSettings _twitchSettings;

        public TwitchChannelService(IOptions<TwitchSettings> twitchSettings)
        {
            _twitchSettings = twitchSettings.Value;
        }

        /// <summary>
        /// Calls the Twitch API to obtain channel info for the named channels.
        /// </summary>
        /// <param name="channelNames"></param>
        /// <returns>The info for each Twitch channel.</returns>
        public async Task<List<TwitchChannel>> GetChannelsInfo(IEnumerable<string> channelNames)
        {
            var channelNamesQueryFormat = string.Join("&login=", channelNames);

            var url = $"{_twitchSettings.BaseApiUrl}/users?login={channelNamesQueryFormat}";
            var jsonResult = await Get(url);

            var result = JsonConvert.DeserializeObject<UserResult>(jsonResult);

            return result.Data.Select(x => x.ToTwitchChannelModel()).ToList();
        }


        /// <summary>
        /// Get basic channel information from the Twitch API, using the name of the channel.
        /// </summary>
        /// <param name="channelName">Name of the channel</param>
        /// <returns></returns>
        public async Task<TwitchChannel> GetChannelInfo(string channelName)
        {
            try
            {
                string url = $"{_twitchSettings.BaseApiUrl}/users?login={channelName}";
                string jsonResult = await Get(url);

                UserResult result = JsonConvert.DeserializeObject<UserResult>(jsonResult);

                UserResultData userInfo = result.Data.SingleOrDefault();
                TwitchChannel channelInfo = userInfo.ToTwitchChannelModel();
                return channelInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
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