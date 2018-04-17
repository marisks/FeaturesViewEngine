using FeaturesViewEngine.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.ControllerFeature
{
    public class LayoutTests
    {
        public LayoutTests()
        {
            _sut = new DemoSite();
        }

        private readonly DemoSite _sut;

        [Fact]
        public void ExistingViewWithLayoutBySpecificName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/IndexWithLayoutBySpecificName");
                    response.Should()
                        .MatchEquivalentOf(
                            "*~/Features/ControllerFeature/_Layout.cshtml*/Features/ControllerFeature/Index.cshtml");
                })
                .Should().NotThrow();
        }
    }
}