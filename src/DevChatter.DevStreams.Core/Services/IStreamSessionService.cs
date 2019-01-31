using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Core.Services
{
    public interface IStreamSessionService
    {
        Task<IList<StreamSession>> Get(string timeZoneId, DateTime localDateTime, IEnumerable<int> includedTagIds);
    }
}