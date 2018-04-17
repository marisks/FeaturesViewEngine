using FeaturesViewEngine.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.ControllerFeature
{
    public class ViewTests
    {
        public ViewTests()
        {
            _sut = new DemoSite();
        }

        private readonly DemoSite _sut;

        [Fact]
        public void ExistingViewByAction_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/Index");
                    response.Should().MatchEquivalentOf("*~/Features/ControllerFeature/Index.cshtml*");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewByName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/IndexByName");
                    response.Should().MatchEquivalentOf("*~/Features/ControllerFeature/Index.cshtml*");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewBySpecificName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/IndexBySpecificName");
                    response.Should().MatchEquivalentOf("*~/Features/ControllerFeature/Index.cshtml*");
                })
                .Should().NotThrow();
        }
    }
}