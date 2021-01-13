using System.Collections.Generic;
using System.Net.Http;

namespace AutomaticBuyTicket.Core.Http.ContentTypeAlgorithms
{

    [Content("application/x-www-form-urlencoded")]
    public class FormUrlEncodedContentProccesser : IHttpContentProccesser
    {
        public HttpContent GetHttpContent(object content)
        {
            return new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)content);
        }
    }
}
