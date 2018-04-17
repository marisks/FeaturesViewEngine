using FeaturesViewEngine.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.Default
{
    public class DisplayModeTests
    {
        public DisplayModeTests()
        {
            _sut = new DemoSite();
        }

        private readonly DemoSite _sut;

        [Fact]
        public void ExistingPartialExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/Partial", "Mobile");
                    response.Should().MatchEquivalentOf("*~/Views/Shared/Partial.Mobile.cshtml*");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingPartialNonExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/Partial", "Tablet");
                    response.Should().MatchEquivalentOf("*~/Views/Shared/Partial.cshtml*");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/Index", "Mobile");
                    response.Should().MatchEquivalentOf("*~/Views/Default/Index.Mobile.cshtml*");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewNonExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/Index", "Tablet");
                    response.Should().MatchEquivalentOf("*~/Views/Default/Index.cshtml*");
                })
                .Should().NotThrow();
        }
    }
}