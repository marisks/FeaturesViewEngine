using FeaturesViewEngine.Tests.Helpers;
using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.Default
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
                    var response = await sut.Get("/Default/IndexWithLayoutBySpecificName");
                    response.Should().MatchEquivalentOf("*~/Views/Shared/_Layout.cshtml*/Views/Default/Index.cshtml*");
                })
                .Should().NotThrow();
        }
    }
}