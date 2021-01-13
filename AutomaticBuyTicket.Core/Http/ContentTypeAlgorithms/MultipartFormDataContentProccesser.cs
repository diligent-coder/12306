using System.Net.Http;

namespace AutomaticBuyTicket.Core.Http.ContentTypeAlgorithms
{
    [Content("multipart/form-data")]
    public class MultipartFormDataContentProccesser : IHttpContentProccesser
    {
        public HttpContent GetHttpContent(object content)
        {
            return new MultipartFormDataContent((string)content);
        }
    }
}
