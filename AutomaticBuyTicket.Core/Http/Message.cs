using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticBuyTicket.Core.Http
{
    public abstract class Message
    {
        public string RequestUri { get; set; }
    }
}
