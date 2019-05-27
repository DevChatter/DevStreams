using System.Linq;
using System.Threading.Tasks;
using DevChatter.DevStreams.Core.Twitch;
using FluentAssertions;
using Moq;
using Xunit;

namespace DevChatter.DevStreams.UnitTests.Core.Twitch.TwitchChannelServiceTests
{
    public class GetChannelsInfoShould
    {
        // Sample from https://dev.twitch.tv/docs/api/reference/#get-users
        private const string SAMPLE_JSON = 
@"{
  ""data"": [{
    ""id"": ""44322889"",
    ""login"": ""dallas"",
    ""display_name"": ""dallas"",
    ""type"": ""staff"",
    ""broadcaster_type"": """",
    ""description"": ""Just a gamer playing games and chatting. :)"",
    ""profile_image_url"": ""https://static-cdn.jtvnw.net/jtv_user_pictures/dallas-profile_image-1a2c906ee2c35f12-300x300.png"",
    ""offline_image_url"": ""https://static-cdn.jtvnw.net/jtv_user_pictures/dallas-channel_offline_image-1a2c906ee2c35f12-1920x1080.png"",
    ""view_count"": 191836881,
    ""email"": ""login@provider.com""
  }]
}";
        private readonly Mock<ITwitchApiClient> _mock;

        public GetChannelsInfoShould()
        {
            _mock = new Mock<ITwitchApiClient>();
        }

        [Fact]
        public async Task ReturnSingleChannelInfo_GivenOneChannelName()
        {
            string channelName = "ChannelName";
            string expectedUrl = $"/users?login={channelName}";
            _mock.Setup(x => x.GetJsonData(expectedUrl)).ReturnsAsync(SAMPLE_JSON);
            var service = new TwitchChannelService(_mock.Object);

            var result = await service.GetChannelsInfo(channelName);

            result.Should().ContainSingle();
        }
    }
}