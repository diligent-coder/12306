using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutomaticBuyTicket.Core.Http
{
    public class MyHttpClient
    {
        public static async Task<MyHttpClientResult> PostAsync(PostMessage postMessage)
        {
            MyHttpClientResult result = new MyHttpClientResult();

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.CookieContainer = SiteConfig.Instance.CookieContainer;

                using (HttpClient httpClient = new HttpClient(handler))
                {
                    //添加自定义Header
                    if (postMessage.CustomHeaders != null)
                    {
                        foreach (var header in postMessage.CustomHeaders)
                        {
                            if (!httpClient.DefaultRequestHeaders.Contains(header.Key))
                            {
                                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                            }
                        }
                    }

                    using (HttpContent content = postMessage.GetHttpContent())
                    {
                        var httpResponseMessage = await httpClient.PostAsync(postMessage.RequestUri, content);

                        result.Content = await httpResponseMessage.Content.ReadAsStringAsync();
                        result.StausCode = httpResponseMessage.StatusCode;                        

                        return result;
                    }
                }
            }
        }

        public class MyHttpClientResult
        {
            public string Content { get; set; }

            public HttpStatusCode StausCode { get; set; }
        }
    }
}
