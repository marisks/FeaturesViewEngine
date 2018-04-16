using FluentAssertions;
using Xunit;

namespace FeaturesViewEngine.Tests.Default
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
                    var response = await sut.Get("/Default/Index");
                    response.Should().Match("~/Views/Default/Index.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewByName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/IndexByName");
                    response.Should().Match("~/Views/Default/Index.cshtml");
                })
                .Should().NotThrow();
        }

        [Fact]
        public void ExistingViewBySpecificName_Success()
        {
            _sut.Awaiting(async sut =>
                {
                    var response = await sut.Get("/Default/IndexBySpecificName");
                    response.Should().Match("~/Views/Default/Index.cshtml");
                })
                .Should().NotThrow();
        }
    }
}