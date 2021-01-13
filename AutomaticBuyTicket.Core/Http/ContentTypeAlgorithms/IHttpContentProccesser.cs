using System.Net.Http;

namespace AutomaticBuyTicket.Core.Http.ContentTypeAlgorithms
{
    public interface IHttpContentProccesser
    {
        HttpContent GetHttpContent(object content);
    }
}
