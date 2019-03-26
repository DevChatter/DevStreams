using DevChatter.DevStreams.Core.Twitch;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Caching
{
    public class CachedTwitchService : ITwitchService
    {
        private readonly ITwitchService _service;
        private readonly IMemoryCache _cacheLayer;

        public CachedTwitchService(ITwitchService service, IMemoryCache cacheLayer)
        {
            _service = service;
            _cacheLayer = cacheLayer;
        }

        public Task<List<string>> GetChannelIds(List<string> channelNames)
        {
            return _service.GetChannelIds(channelNames);
        }

        public async Task<List<string>> GetLiveChannels(List<string> channelNames)
        {
            return await _cacheLayer.GetOrCreateAsync("AllLiveChannels", async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                return await GetLiveChannelsFallback(channelNames);
            });
        }

        private string GetKey(string twitchId) => $"Twitch-LiveStatus-{twitchId}";

        public async Task<List<string>> GetLiveChannelsFallback(List<string> channelNames)
        {
            return await _service.GetLiveChannels(channelNames);
        }

        public async Task<bool> IsLive(string twitchId)
        {
            return await _cacheLayer.GetOrCreateAsync($"{twitchId}", async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                return await IsLiveFallback(twitchId);
            });
        }

        public async Task<bool> IsLiveFallback(string key) => await _service.IsLive(key);
    }
}