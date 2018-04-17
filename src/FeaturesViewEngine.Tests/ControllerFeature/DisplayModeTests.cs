using FeaturesViewEngine.Tests.Helpers;
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
        public void ExistingPartialExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/Partial", "Mobile");
                    response.Should().MatchEquivalentOf("*~/Features/ControllerFeature/Partial.Mobile.cshtml*");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingPartialNonExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/Partial", "Tablet");
                    response.Should().MatchEquivalentOf("*~/Features/ControllerFeature/Partial.cshtml*");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/Index", "Mobile");
                    response.Should().MatchEquivalentOf("*~/Features/ControllerFeature/Index.Mobile.cshtml*");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewNonExistingDisplayMode_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/Index", "Tablet");
                    response.Should().MatchEquivalentOf("*~/Features/ControllerFeature/Index.cshtml*");
                })
                .Should().NotThrow();
        }
    }
}