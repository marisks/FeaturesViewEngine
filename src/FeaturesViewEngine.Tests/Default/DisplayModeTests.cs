using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.Default
{
    public class DisplayModeTests
    {
        public DisplayModeTests()
        {
            _sut = new DemoSite("http://localhost:5000");
        }

        private readonly DemoSite _sut;

        [Fact]
        public void ExistingViewExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/Index", "Custom");
                    response.Should().Match("~/Views/Default/Index.Custom.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewNonExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/Index", "Custom");
                    response.Should().Match("~/Views/Default/Index.Custom.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingPartialExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/Partial", "Custom");
                    response.Should().Match("~/Views/Shared/Partial.Custom.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingPartialNonExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/Partial", "Custom");
                    response.Should().Match("~/Views/Shared/Partial.Custom.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewExistingLayoutExistingPartialByName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/IndexWithLayoutWithPartialByName", "Custom");
                    response.Should()
                        .MatchEquivalentOf(
                            "*~/Views/Shared/_Layout.Custom.cshtml*~/Views/Default/IndexWithPartialByName.cshtml*~/Views/Shared/Partial.Custom.cshtml*");
                })
                .Should().NotThrow();
        }

    }
}