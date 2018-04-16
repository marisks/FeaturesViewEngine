using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.Default
{
    public class LayoutTests
    {
        public LayoutTests()
        {
            _sut = new DemoSite("http://localhost:5000");
        }

        private readonly DemoSite _sut;

        [Fact]
        public void ExistingViewWithLayoutByName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/IndexWithLayoutByName");
                    response.Should().MatchEquivalentOf("*~/Views/Shared/_Layout.cshtml*/Views/Default/Index.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewWithLayoutBySpecificName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/IndexWithLayoutBySpecificName");
                    response.Should().MatchEquivalentOf("*~/Views/Shared/_Layout.cshtml*/Views/Default/Index.cshtml");
                })
                .Should().NotThrow();
        }
    }
}