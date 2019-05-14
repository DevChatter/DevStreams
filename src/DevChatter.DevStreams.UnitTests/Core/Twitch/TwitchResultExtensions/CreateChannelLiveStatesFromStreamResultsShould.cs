using System;
using System.Collections.Generic;
using System.Linq;
using DevChatter.DevStreams.Core.Twitch;
using FluentAssertions;
using NodaTime;
using NodaTime.Text;
using Xunit;

namespace DevChatter.DevStreams.UnitTests.Core.Twitch.TwitchResultExtensions
{
    public class CreateChannelLiveStatesFromStreamResultsShould
    {
        [Fact]
        public void ReturnEmpty_GivenNoResults()
        {
            var streamResult = new StreamResult { Data = new List<StreamResultData>() };
            var twitchIds = new List<string>();

            var liveStates = streamResult.CreateChannelLiveStatesFromStreamResults(twitchIds);

            liveStates.Should().BeEmpty();
        }

        [Fact]
        public void ReturnNonLiveResult_GivenNotLiveChannel()
        {
            const string twitchId = "123Link";
            var streamResult = new StreamResult {Data = new List<StreamResultData>()};
            var twitchIds = new List<string> { twitchId };

            var liveStates = streamResult.CreateChannelLiveStatesFromStreamResults(twitchIds);

            liveStates.Single().IsLive.Should().BeFalse();
            liveStates.Single().TwitchId.Should().Be(twitchId);
            liveStates.Single().StartedAt.Should().Be(Instant.MinValue);
            liveStates.Single().ViewerCount.Should().Be(0);
        }

        [Fact]
        public void ReturnLiveResultWithData_GivenLiveChannel()
        {
            const string twitchId = "DevChatter42";
            var startedAt = InstantPattern.General.Parse("2019-05-04T17:04:19Z").Value;
            const int viewerCount = 77;
            var streamResult = new StreamResult {Data = new List<StreamResultData>
            {
                new StreamResultData
                {
                    Started_at = startedAt,
                    Viewer_count = viewerCount,
                    User_id = twitchId,
                }
            }};
            var twitchIds = new List<string> { twitchId };

            var liveState = streamResult.CreateChannelLiveStatesFromStreamResults(twitchIds)
                .Single();

            liveState.IsLive.Should().BeTrue();
            liveState.TwitchId.Should().Be(twitchId);
            liveState.StartedAt.Should().Be(startedAt);
            liveState.ViewerCount.Should().Be(viewerCount);
        }
    }
}