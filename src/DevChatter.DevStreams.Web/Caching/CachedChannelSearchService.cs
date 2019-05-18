using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Core.Services;
using DevChatter.DevStreams.Core.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Web.Caching
{
    public class CachedChannelSearchService : IChannelSearchService
    {
        private readonly IChannelSearchService _searchService;
        private readonly IMemoryCache _cacheLayer;
        private readonly CacheSettings _settings;

        public CachedChannelSearchService(IChannelSearchService searchService,
            IMemoryCache cacheLayer, IOptions<CacheSettings> cacheSettings)
        {
            _searchService = searchService;
            _cacheLayer = cacheLayer;
            _settings = cacheSettings.Value;
        }

        public async Task<Channel> GetChannelSoundex(string standardizedChannelName)
        {
            string cacheKey = $"{nameof(IChannelSearchService)}-{nameof(GetChannelSoundex)}-{standardizedChannelName}";

            var channel = await _cacheLayer.GetOrCreateAsync(cacheKey, CacheFallback);

            return channel;

            Task<Channel> CacheFallback(ICacheEntry entry)
            {
                var expirationTime = TimeSpan.FromMinutes(_settings.MediumCacheMinutes);
                entry.SetAbsoluteExpiration(expirationTime);
                return _searchService.GetChannelSoundex(standardizedChannelName);
            }
        }

        public async Task<List<Channel>> GetChannelsByTagMatches(params int[] tagIds)
        {
            string tags = string.Join("-", tagIds);
            string cacheKey = $"{nameof(IChannelSearchService)}-{nameof(Task)}-{tags}";

            var channels = await _cacheLayer.GetOrCreateAsync(cacheKey, CacheFallback);

            return channels;

            Task<List<Channel>> CacheFallback(ICacheEntry entry)
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(15)); // TODO: Store in Config Setting
                return _searchService.GetChannelsByTagMatches(tagIds);
            }
        }
    }
}