using AutomaticBuyTicket.Core.Http;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace AutomaticBuyTicket.Core.Auth
{
    /// <summary>
    /// 扫描二维码登录
    /// </summary>
    public class LoginByQRCode
    {
        public const string CreateQrcodeUrl = "https://kyfw.12306.cn/passport/web/create-qr64";

        public const string CheckQrcodeUrl = "https://kyfw.12306.cn/passport/web/checkqr";

        public const string AuthUrl = "https://kyfw.12306.cn/passport/web/auth/uamtk";

        public const string ClientAuthUrl = "https://kyfw.12306.cn/otn/uamauthclient";

        public static CreateQRCodeResult QRCodeResult;

        public static CheckQRCodeResult CheckQRCodeResult;

        public static AuthResult AuthResult;

        public static ClientAuthResult ClientAuthResult;

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
                Cookies = PostMessageExtention.Cookies.ToList()
            };

            await postMessage.Post<CreateQRCodeResult>(result=>
            {
                if (result.result_code == "0")
                {
                    QRCodeResult = result;
                }
            });
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
                Cookies = PostMessageExtention.Cookies.ToList()
            };

            await postMessage.Post<CheckQRCodeResult>(result =>
            {
                CheckQRCodeResult = result;
            });           
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
                Cookies = PostMessageExtention.Cookies.ToList()
            };
            postMessage.CustomHeaders.Add("Referer", ConfigurationManager.AppSettings["Referer"]);
            postMessage.CustomHeaders.Add("Origin", ConfigurationManager.AppSettings["Origin"]);

            await postMessage.Post<AuthResult>(result =>
            {
                if (result.result_code == "0")
                {
                    AuthResult = result;
                }
            });
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
                Cookies = PostMessageExtention.Cookies.ToList()
            };

            await postMessage.Post<ClientAuthResult>( result =>
            {
                if (result.result_code == "0")
                {
                    ClientAuthResult = result;
                }
            });
        }
    }
}



