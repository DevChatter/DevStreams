using DevChatter.DevStreams.Core.Twitch;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
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

        public Task<List<string>> GetLiveChannels(List<string> channelNames)
        {
            return _service.GetLiveChannels(channelNames);
        }

        public async Task<bool> IsLive(int twitchId)
        {
            return await _cacheLayer.GetOrCreateAsync($"{twitchId}", async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                return await IsLiveFallback(twitchId);
            });
        }

        public async Task<bool> IsLiveFallback(int key) => await _service.IsLive(key);
    }
}