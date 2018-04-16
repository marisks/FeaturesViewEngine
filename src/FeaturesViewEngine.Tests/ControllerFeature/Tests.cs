using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.ControllerFeature
{
    public class Tests
    {
        public Tests()
        {
            _sut = new DemoSite("http://localhost:5000");
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

        [Fact]
        public void ExistingViewByAction_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/Index");
                    response.Should().Match("~/Features/ControllerFeature/Index.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewByName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/IndexByName");
                    response.Should().Match("~/Features/ControllerFeature/Index.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewBySpecificName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/ControllerFeature/IndexBySpecificName");
                    response.Should().Match("~/Features/ControllerFeature/Index.cshtml");
                })
                .Should().NotThrow();
        }
    }
}