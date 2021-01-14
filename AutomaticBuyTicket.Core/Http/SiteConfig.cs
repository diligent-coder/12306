using System;
using System.Net;

namespace AutomaticBuyTicket.Core.Http
{
    public class SiteConfig
    {
        private SiteConfig()
        {

        }

        private static readonly Lazy<SiteConfig> lazy =new Lazy<SiteConfig>(() => new SiteConfig());

        public readonly CookieContainer CookieContainer = new CookieContainer();

        public static SiteConfig Instance
        {
            get
            {              
                return lazy.Value;
            }
        }
    }
}
