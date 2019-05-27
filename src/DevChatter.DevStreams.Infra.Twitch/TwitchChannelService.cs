using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Twitch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Twitch
{
    public class TwitchChannelService : ITwitchChannelService
    {
        private readonly ITwitchApiClient _twitchApiClient;

        public TwitchChannelService(ITwitchApiClient twitchApiClient)
        {
            _twitchApiClient = twitchApiClient;
        }

        /// <summary>
        /// Calls the Twitch API to obtain channel info for the named channels.
        /// </summary>
        /// <param name="channelNames"></param>
        /// <returns>The info for each Twitch channel.</returns>
        public async Task<List<TwitchChannel>> GetChannelsInfo(IEnumerable<string> channelNames)
        {
            var channelNamesQueryFormat = string.Join("&login=", channelNames);

            var url = $"/users?login={channelNamesQueryFormat}";
            string jsonResult = await _twitchApiClient.GetJsonData(url);

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
                string url = $"/users?login={channelName}";
                string jsonResult = await _twitchApiClient.GetJsonData(url);

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
    }
}