using System;
using DevChatter.DevStreams.Core.Twitch;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Caching;
using FluentAssertions;
using Xunit;

namespace DevChatter.DevStreams.UnitTests.Core.Twitch.CachedTwitchServiceTests
{
    public class IsLiveShould
    {
        const int twitchId = 1337;

        public IsLiveShould()
        {
        }

        [Fact]
        public void NotCallTwitch_WhenValueIsCached()
        {
            var fakeService = new FakeTwitchService();
            var fakeCacheLayer = new FakeCacheLayer {Cache = {[twitchId] = true}};
            var twitchService = new CachedTwitchService(fakeService, fakeCacheLayer);

            bool isLive = twitchService.IsLive(twitchId).Result;

            fakeService.IsLiveCalled.Should().BeFalse();
        }

    }

    public class FakeCacheLayer : ICacheLayer<int,bool>
    {
        public Dictionary<int,bool> Cache { get; set; } = new Dictionary<int, bool>();
        public async Task<bool> GetValueOrFallback(int key, Func<int, Task<bool>> fallBack)
        {
            if (Cache.TryGetValue(key, out bool value))
            {
                return value;
            }

            return await fallBack.Invoke(key);
        }
    }

    public class FakeTwitchService : ITwitchService
    {
        public Task<List<string>> GetChannelIds(List<string> channelNames)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<string>> GetLiveChannels(List<string> channelNames)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsLive(int twitchId)
        {
            IsLiveCalled = true;
            return Task.FromResult(true);
        }

        public bool IsLiveCalled { get; set; } = false;
    }
}