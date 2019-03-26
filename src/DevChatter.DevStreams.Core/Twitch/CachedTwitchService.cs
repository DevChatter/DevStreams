using DevChatter.DevStreams.Core.Caching;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Twitch
{
    public class CachedTwitchService : ITwitchService
    {
        private readonly ITwitchService _service;
        private readonly ICacheLayer<int,bool> _cacheLayer;

        public CachedTwitchService(ITwitchService service, ICacheLayer<int,bool> cacheLayer)
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
            return await _cacheLayer.GetValueOrFallback(twitchId, IsLiveFallback);
        }

        public async Task<bool> IsLiveFallback(int key) => await _service.IsLive(key);
    }
}