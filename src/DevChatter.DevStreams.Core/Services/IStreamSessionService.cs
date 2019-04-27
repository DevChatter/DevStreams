using DevChatter.DevStreams.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Services
{
    public interface IStreamSessionService
    {
        Task<List<EventResult>> Get(string timeZoneId, DateTime localDateTime, List<int> tagIds);

        Task<IDictionary<int, StreamSession>> GetChannelNextStreamLookup(IEnumerable<int> channelIds);

        Task<ILookup<int, StreamSession>> GetChannelFutureStreamsLookup(IEnumerable<int> channelIds);
    }
}