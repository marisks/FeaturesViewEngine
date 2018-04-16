using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.Default
{
    public class PartialTests
    {
        public PartialTests()
        {
            _sut = new DemoSite("http://localhost:5000");
        }

        private readonly DemoSite _sut;

        [Fact]
        public void ExistingPartial_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/Partial");
                    response.Should().Match("~/Views/Shared/Partial.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingPartialByName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/PartialByName");
                    response.Should().Match("~/Views/Shared/Partial.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingPartialBySpecificName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/PartialBySpecificName");
                    response.Should().Match("~/Views/Shared/Partial.cshtml");
                })
                .Should().NotThrow();
        }
    }
}