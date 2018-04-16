using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.ControllerFeature
{
    public class PartialTests
    {
        public PartialTests()
        {
            _sut = new DemoSite();
        }

        private readonly DemoSite _sut;

        [Fact]
        public void ExistingPartial_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/Partial");
                    response.Should().Match("~/Features/ControllerFeature/Partial.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingPartialByName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/PartialByName");
                    response.Should().Match("~/Features/ControllerFeature/Partial.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingPartialBySpecificName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/PartialBySpecificName");
                    response.Should().Match("~/Features/ControllerFeature/Partial.cshtml");
                })
                .Should().NotThrow();
        }
    }
}