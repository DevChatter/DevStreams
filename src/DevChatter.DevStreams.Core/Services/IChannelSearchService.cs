using System.Collections.Generic;
using DevChatter.DevStreams.Core.Model;

namespace DevChatter.DevStreams.Core.Services
{
    public interface IChannelSearchService
    {
        List<Channel> Find();
    }
}