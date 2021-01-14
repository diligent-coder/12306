using AutomaticBuyTicket.Core.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AutomaticBuyTicket.Core.Http
{
    public static class PostMessageExtention
    {
        public static ConcurrentBag<Cookie> Cookies = new ConcurrentBag<Cookie>();

        public static async Task Post<T>(this PostMessage postMessage, Action<T> action) where T : CommonResult
        {
            var postResult = await MyHttpClient.PostAsync(postMessage);
            if (postResult.Cookies != null)
            {
                foreach (var cookie in postResult.Cookies)
                {
                    var getCookie = Cookies.FirstOrDefault(c => c.Name == cookie.Name);
                    if (getCookie != null)
                    {
                        if (getCookie.Value != cookie.Value)
                        {
                            getCookie.Value = cookie.Value;
                        }
                    }
                    else
                    {
                        Cookies.Add(cookie);
                    }
                }
            }

            var result = JsonConvert.DeserializeObject<T>(postResult.Content);

            action?.Invoke(result);
        }
    }
}
