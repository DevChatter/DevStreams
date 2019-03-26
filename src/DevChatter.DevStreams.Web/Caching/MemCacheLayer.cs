using System;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace DevChatter.DevStreams.Web.Caching
{
    public class MemCacheLayer<TKey, TValue> : ICacheLayer<TKey, TValue>
    {
        private readonly IMemoryCache _memoryCache;

        public MemCacheLayer(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<TValue> GetValueOrFallback(TKey key, Func<TKey, Task<TValue>> fallBack)
        {
            throw new NotImplementedException();
        }
    }
}