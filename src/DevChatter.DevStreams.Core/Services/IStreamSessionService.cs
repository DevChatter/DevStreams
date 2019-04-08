using DevChatter.DevStreams.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Services
{
    public interface IStreamSessionService
    {
        Task<List<EventResult>> Get(string timeZoneId, DateTime localDateTime, IEnumerable<int> includedTagIds);

        Task<IDictionary<int, StreamSession>> GetChannelNextStreamLookup(IEnumerable<int> channelIds);
    }
}