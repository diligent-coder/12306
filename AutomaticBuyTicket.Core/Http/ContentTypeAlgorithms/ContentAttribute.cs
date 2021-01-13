using System;

namespace AutomaticBuyTicket.Core.Http.ContentTypeAlgorithms
{
    public class ContentAttribute : Attribute
    {
        public string ContentType { get; set; }

        public ContentAttribute(string contentType)
        {
            ContentType = contentType;
        }
    }
}
