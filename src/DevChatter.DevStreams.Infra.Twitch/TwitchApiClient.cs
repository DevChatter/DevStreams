using DevChatter.DevStreams.Core.Settings;
using DevChatter.DevStreams.Core.Twitch;
using Flurl;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Infra.Twitch
{
    public class TwitchApiClient : ITwitchApiClient
    {
        private readonly TwitchSettings _twitchSettings;

        public TwitchApiClient(IOptions<TwitchSettings> twitchSettings)
        {
            _twitchSettings = twitchSettings.Value;
        }

        public async Task<string> GetJsonData(string url)
        {
            string fullUrl = Url.Combine(_twitchSettings.BaseApiUrl, url);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Client-Id", _twitchSettings.ClientId);
                var result = await client.GetStringAsync(fullUrl);
                return result;
            }
        }
    }
}