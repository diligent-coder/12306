using AutomaticBuyTicket.Core.Http.ContentTypeAlgorithms;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;

namespace AutomaticBuyTicket.Core.Http
{
    public class PostMessage : Message
    {

        private IHttpContentProccesser _httpContentProccesser;

        public object Content { get; set; }

        public string ContentType { get; set; } = "application/json";

        public Dictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string, string>
        {
            { "User-Agent",ConfigurationManager.AppSettings["UserAgent"] }
        };      

        public HttpContent GetHttpContent()
        {
            _httpContentProccesser = HttpContentFactory.GetContentProccesser(ContentType);
            HttpContent httpContent = _httpContentProccesser.GetHttpContent(Content);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            return httpContent;
        }

        public List<Cookie> Cookies { get; set; }
    }

}
