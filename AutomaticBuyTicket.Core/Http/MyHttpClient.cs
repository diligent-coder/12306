using System;
using System.Collections.Generic;
using System.Configuration;
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
                if (postMessage.Cookies != null)
                {
                    handler.CookieContainer = new CookieContainer();
                    foreach (var cookie in postMessage.Cookies)
                    {
                        handler.CookieContainer.Add(cookie);
                    }
                }

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
                        httpResponseMessage.Headers.TryGetValues("Set-Cookie", out var cookies);


                        if (cookies != null)
                        {
                            List<Cookie> cookieList = new List<Cookie>();
                            result.Cookies = cookieList;
                            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                            foreach (var cookie in cookies)
                            {
                                var cookieContents = cookie.Split(new[] { ';', '=' }, StringSplitOptions.RemoveEmptyEntries);
                                cookieList.Add(new Cookie(cookieContents[0], cookieContents[1],"/"
                                    ,ConfigurationManager.AppSettings["Domain"]));
                            }
                        }

                        return result;
                    }
                }
            }
        }

        public class MyHttpClientResult
        {
            public IEnumerable<Cookie> Cookies { get; set; }

            public string Content { get; set; }


            public HttpStatusCode StausCode { get; set; }
        }
    }
}
