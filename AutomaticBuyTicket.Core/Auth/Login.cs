using AutomaticBuyTicket.Core.Http;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static AutomaticBuyTicket.Core.Http.MyHttpClient;

namespace AutomaticBuyTicket.Core.Auth
{
    public class Login
    {
        public const string CreateQrcodeUrl = "https://kyfw.12306.cn/passport/web/create-qr64";

        public const string CheckQrcodeUrl = "https://kyfw.12306.cn/passport/web/checkqr";

        public const string AuthUrl = "https://kyfw.12306.cn/passport/web/auth/uamtk";

        public const string ClientAuthUrl = "https://kyfw.12306.cn/otn/uamauthclient";

        public static CreateQRCodeResult QRCodeResult;

        public static CheckQRCodeResult CheckQRCodeResult;

        public static AuthResult AuthResult;

        public static ClientAuthResult ClientAuthResult;

        public static ConcurrentBag<Cookie> Cookies = new ConcurrentBag<Cookie>();

        /// <summary>
        /// 获取二维码图片
        /// </summary>
        /// <returns></returns>
        public static async Task GetQRCodeAsync()
        {
            PostMessage postMessage = new PostMessage()
            {
                ContentType = "application/x-www-form-urlencoded",
                RequestUri = CreateQrcodeUrl,
                Content = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("appid", "otn")
                },
                Cookies = Cookies.ToList()
            };

            MyHttpClientResult postResult = await SetCookie(postMessage);
            var result = JsonConvert.DeserializeObject<CreateQRCodeResult>(postResult.Content);
            if (result.result_code == "0")
            {
                QRCodeResult = result;
            }

        }

        /// <summary>
        /// 检查二维码扫码状态
        /// </summary>
        /// <returns></returns>
        public static async Task CheckQRCodeAsync()
        {
            PostMessage postMessage = new PostMessage()
            {
                ContentType = "application/x-www-form-urlencoded",
                RequestUri = CheckQrcodeUrl,
                Content = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("appid", "otn"),
                    new KeyValuePair<string, string>("uuid",QRCodeResult.uuid),
                    new KeyValuePair<string, string>("RAIL_DEVICEID",GetDeviceID()),
                    new KeyValuePair<string, string>("RAIL_EXPIRATION",GetExpiration()),
                },
                Cookies = Cookies.ToList()
            };

            MyHttpClientResult postResult = await SetCookie(postMessage);
            var result = JsonConvert.DeserializeObject<CheckQRCodeResult>(postResult.Content);

            CheckQRCodeResult = result;
        }

        private static async Task<MyHttpClient.MyHttpClientResult> SetCookie(PostMessage postMessage)
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

            return postResult;
        }

        public static string GetDeviceID()
        {
            return ConfigurationManager.AppSettings["RAIL_DEVICEID"];
        }

        public static string GetExpiration()
        {
            return ConfigurationManager.AppSettings["RAIL_EXPIRATION"];
        }

        /// <summary>
        /// 第一次检测用户状态
        /// </summary>
        /// <returns></returns>
        public static async Task AuthAsync()
        {
            PostMessage postMessage = new PostMessage()
            {
                ContentType = "application/x-www-form-urlencoded",
                RequestUri = AuthUrl,
                Content = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("appid", "otn")
                },
                Cookies = Cookies.ToList()
            };
            postMessage.CustomHeaders.Add("Referer", ConfigurationManager.AppSettings["Referer"]);
            postMessage.CustomHeaders.Add("Origin", ConfigurationManager.AppSettings["Origin"]);

            MyHttpClientResult postResult = await SetCookie(postMessage);
            var result = JsonConvert.DeserializeObject<AuthResult>(postResult.Content);
            if (result.result_code == "0")
            {
                AuthResult = result;
            }
        }

        /// <summary>
        /// 第二次检测用户状态
        /// </summary>
        /// <returns></returns>
        public static async Task ClientAuthAsync()
        {
            PostMessage postMessage = new PostMessage()
            {
                ContentType = "application/x-www-form-urlencoded",
                RequestUri = ClientAuthUrl,
                Content = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("tk", AuthResult.newapptk)
                },
                Cookies = Cookies.ToList()
            };

            MyHttpClientResult postResult = await SetCookie(postMessage);
            var result = JsonConvert.DeserializeObject<ClientAuthResult>(postResult.Content);
            if (result.result_code == "0")
            {
                ClientAuthResult = result;
            }
        }

    }
}

public class CommonResult
{
    public string result_message { get; set; }
    public string result_code { get; set; }
}

public class CreateQRCodeResult : CommonResult
{
    public string image { get; set; }

    public string uuid { get; set; }
}

public class CheckQRCodeResult : CommonResult
{
    public string uamtk { get; set; }
}

public class AuthResult : CommonResult
{
    public string apptk { get; set; }

    public string newapptk { get; set; }
}

public class ClientAuthResult : CommonResult
{
    public string apptk { get; set; }

    public string username { get; set; }
}

