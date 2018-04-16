using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.Default
{
    public class CombinedTests
    {
        public CombinedTests()
        {
            _sut = new DemoSite();
        }

        private readonly DemoSite _sut;

        [Fact]
        public void ExistingViewExistingLayoutExistingPartialByName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/IndexWithLayoutWithPartialByName");
                    response.Should()
                        .MatchEquivalentOf(
                            "*~/Views/Shared/_Layout.cshtml*~/Views/Default/IndexWithPartialByName.cshtml*~/Views/Shared/Partial.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewExistingLayoutExistingPartialBySpecificName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/IndexWithLayoutWithPartialBySpecificName");
                    response.Should()
                        .MatchEquivalentOf(
                            "*~/Views/Shared/_Layout.cshtml*~/Views/Default/IndexWithPartialBySpecificName.cshtml*~/Views/Shared/Partial.cshtml");
                })
                .Should().NotThrow();
        }
    }
}