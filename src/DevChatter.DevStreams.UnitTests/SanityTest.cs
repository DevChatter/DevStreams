using FluentAssertions;
using Xunit;

namespace DevChatter.DevStreams.UnitTests
{
    public class SanityTest
    {
        [Fact]
        public void TrueShouldBeTrue()
        {
            const bool truth = true;
            truth.Should().BeTrue();
        }
    }
}