using System.Threading.Tasks;
using System.Web.Configuration;
using Flurl;
using Flurl.Http;

namespace FeaturesViewEngine.Tests.Helpers
{
    public class DemoSite
    {
        private readonly string _baseUrl;

        public DemoSite() : this(WebConfigurationManager.AppSettings.Get("DemoSite:BaseUrl"))
        {
        }

        private DemoSite(string baseUrl)
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