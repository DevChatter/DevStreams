using System;
using DevChatter.DevStreams.Core.Twitch;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Caching;
using FluentAssertions;
using Xunit;
using Moq;

namespace DevChatter.DevStreams.UnitTests.Core.Twitch.CachedTwitchServiceTests
{
    public class IsLiveShould
    {
        private readonly Mock<ICacheLayer<int, bool>> _cacheMock;
        private readonly Mock<ITwitchService> _twitchMock;
        const int twitchId = 1337;

        public IsLiveShould()
        {
            _cacheMock = new Mock<ICacheLayer<int,bool>>();
            _twitchMock = new Mock<ITwitchService>();
        }

        [Fact]
        public void NotCallTwitch_WhenValueIsCached()
        {
            _cacheMock.Setup(x => 
                    x.GetValueOrFallback(twitchId, It.IsAny<Func<int, Task<bool>>>()))
                .Returns(Task.FromResult(true));
            var twitchService = new CachedTwitchService(_twitchMock.Object, _cacheMock.Object);

            bool isLive = twitchService.IsLive(twitchId).Result;

            _twitchMock.Verify(x => x.IsLive(twitchId), Times.Never);
        }
    }
}