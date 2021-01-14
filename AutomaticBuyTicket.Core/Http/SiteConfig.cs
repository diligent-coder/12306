using System.Net;

namespace AutomaticBuyTicket.Core.Http
{
    public class SiteConfig
    {
        private static SiteConfig _siteConfig;

        public CookieContainer CookieContainer = new CookieContainer();

        public static SiteConfig Instance
        {
            get
            {
                if (_siteConfig == null)
                {
                    _siteConfig = new SiteConfig();
                }
                return _siteConfig;
            }
        }
    }
}
