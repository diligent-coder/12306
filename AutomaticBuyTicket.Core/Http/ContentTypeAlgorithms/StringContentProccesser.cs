using System.Net.Http;

namespace AutomaticBuyTicket.Core.Http.ContentTypeAlgorithms
{
    [Content("application/json")]
    public class StringContentProccesser : IHttpContentProccesser
    {
        public HttpContent GetHttpContent(object content)
        {
            return new StringContent((string)content);
        }
    }
}
