using DevChatter.DevStreams.Core.Twitch;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Caching
{
    public class CachedTwitchStreamService : ITwitchStreamService
    {
        private readonly ITwitchStreamService _service;
        private readonly IMemoryCache _cacheLayer;

        public CachedTwitchStreamService(ITwitchStreamService service, IMemoryCache cacheLayer)
        {
            _service = service;
            _cacheLayer = cacheLayer;
        }

        public async Task<List<ChannelLiveState>> GetChannelLiveStates(List<string> twitchIds)
        {
            List<ChannelLiveState> channelLiveStates = new List<ChannelLiveState>();

            List<string> idsNotInCache = new List<string>();

            foreach (var twitchId in twitchIds)
            {
                if (_cacheLayer.TryGetValue(CreateCacheKey(twitchId), out ChannelLiveState state))
                {
                    channelLiveStates.Add(state);
                }
                else
                {
                    idsNotInCache.Add(twitchId);
                }
            }

            List<ChannelLiveState> nonCachedStates = await _service.GetChannelLiveStates(idsNotInCache);

            foreach (var state in nonCachedStates)
            {
                _cacheLayer.Set(CreateCacheKey(state.TwitchId), state);
            }

            return channelLiveStates;
        }


        public async Task<bool> IsLive(string twitchId)
        {
            return await _cacheLayer.GetOrCreateAsync(CreateCacheKey(twitchId), async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                return await IsLiveFallback(twitchId);
            });
        }

        public async Task<bool> IsLiveFallback(string key) => await _service.IsLive(key);

        private string CreateCacheKey(string twitchId) => $"Twitch-LiveStatus-{twitchId}";
    }
}