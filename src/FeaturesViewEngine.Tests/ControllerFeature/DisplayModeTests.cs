using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.ControllerFeature
{
    public class DisplayModeTests
    {
        public DisplayModeTests()
        {
            _sut = new DemoSite();
        }

        private readonly DemoSite _sut;

        [Fact]
        public void ExistingViewExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/Index", "Custom");
                    response.Should().Match("~/Features/ControllerFeature/Index.Custom.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewNonExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/Index");
                    response.Should().Match("~/Features/ControllerFeature/Index.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingPartialExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/Partial", "Custom");
                    response.Should().Match("~/Features/ControllerFeature/Partial.Custom.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingPartialNonExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/Partial");
                    response.Should().Match("~/Features/ControllerFeature/Partial.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewExistingLayoutExistingPartialExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/IndexWithLayoutWithPartialByName", "Custom");
                    response.Should()
                        .MatchEquivalentOf(
                            "*~/Views/Shared/_Layout.Custom.cshtml*~/Features/ControllerFeature/IndexWithPartialByName.cshtml*~/Features/ControllerFeature/Partial.Custom.cshtml*");
                })
                .Should().NotThrow();
        }

    }
}