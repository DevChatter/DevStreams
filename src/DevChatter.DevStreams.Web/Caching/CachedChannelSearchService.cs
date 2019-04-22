using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Caching
{
    public class CachedChannelSearchService : IChannelSearchService
    {
        private readonly IChannelSearchService _searchService;
        private readonly IMemoryCache _cacheLayer;

        public CachedChannelSearchService(IChannelSearchService searchService, IMemoryCache cacheLayer)
        {
            _searchService = searchService;
            _cacheLayer = cacheLayer;
        }

        public async Task<Channel> GetChannelSoundex(string standardizedChannelName)
        {
            string cacheKey = $"{nameof(IChannelSearchService)}-{nameof(GetChannelSoundex)}-{standardizedChannelName}";

            var channel = await _cacheLayer.GetOrCreateAsync(cacheKey, CacheFallback);

            return channel;

            Task<Channel> CacheFallback(ICacheEntry entry)
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(15)); // TODO: Store in Config Setting
                return _searchService.GetChannelSoundex(standardizedChannelName);
            }
        }
    }
}