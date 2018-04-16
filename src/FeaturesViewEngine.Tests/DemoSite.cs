using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace FeaturesViewEngine.Tests
{
    public class DemoSite
    {
        private readonly string _baseUrl;

        public DemoSite(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<string> Get(string path, string displayMode = null)
        {
            return await _baseUrl
                .AppendPathSegment(path)
                .WithHeader("DisplayMode", displayMode)
                .GetStringAsync();
        }
    }
}