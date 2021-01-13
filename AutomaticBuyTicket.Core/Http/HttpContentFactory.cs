using AutomaticBuyTicket.Core.Http.ContentTypeAlgorithms;

namespace AutomaticBuyTicket.Core.Http
{
    public class HttpContentFactory
    {
        public static IHttpContentProccesser GetContentProccesser(string contentType)
        {
            return ContentTypeConfig.Instance.ContentTypeAlgorithm[contentType];
        }
    }
}
