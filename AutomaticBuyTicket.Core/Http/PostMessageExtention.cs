using AutomaticBuyTicket.Core.Auth;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AutomaticBuyTicket.Core.Http
{
    public static class PostMessageExtention
    {
        public static async Task Post<T>(this PostMessage postMessage, Action<T> action) where T : CommonResult
        {
            var postResult = await MyHttpClient.PostAsync(postMessage);
            
            var result = JsonConvert.DeserializeObject<T>(postResult.Content);

            action?.Invoke(result);
        }
    }
}
