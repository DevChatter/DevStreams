using System;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Caching
{
    public interface ICacheLayer<TKey,TValue>
    {
        Task<TValue> GetValueOrFallback(TKey key, Func<TKey, Task<TValue>> fallBack);
    }

}