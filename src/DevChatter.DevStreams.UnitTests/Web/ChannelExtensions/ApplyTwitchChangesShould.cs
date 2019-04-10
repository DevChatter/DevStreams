using DevChatter.DevStreams.Core.Model;
using DevChatter.DevStreams.Web.Data.ViewModel.Channels;
using FluentAssertions;
using Xunit;

namespace DevChatter.DevStreams.UnitTests.Web.ChannelExtensions
{
    public class ApplyTwitchChangesShould
    {
        [Fact]
        public void AddTwitchChannelIfNeeded()
        {
            const string twitchName = "TheTwitchName";
            var channel = new Channel();

            channel.ApplyTwitchChanges(new TwitchChannel { TwitchName = twitchName });

            channel.Twitch.Should().NotBeNull();
            channel.Twitch.TwitchName.Should().Be(twitchName);
        }

        [Fact]
        public void UseChannelIdOnTwitchChannel()
        {
            const int id = 1337;
            var channel = new Channel { Id = id };

            channel.ApplyTwitchChanges(new TwitchChannel());

            channel.Twitch.Should().NotBeNull();
            channel.Twitch.ChannelId.Should().Be(id);
        }
    }
}