using AutomaticBuyTicket.Core.Http.ContentTypeAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutomaticBuyTicket.Core.Http
{                    
    public class ContentTypeConfig
    {
        public readonly Dictionary<string, IHttpContentProccesser> ContentTypeAlgorithm;

        private static readonly Lazy<ContentTypeConfig> lazy = new Lazy<ContentTypeConfig>(() => new ContentTypeConfig());

        private ContentTypeConfig()
        {
            ContentTypeAlgorithm = Assembly.GetAssembly(typeof(IHttpContentProccesser)).GetTypes()
                        .Where(t => t.GetInterfaces().Contains(typeof(IHttpContentProccesser)))
                        .ToDictionary(t => t.GetCustomAttribute<ContentAttribute>().ContentType
                            , t => (IHttpContentProccesser)Activator.CreateInstance(t));
        }

        public static ContentTypeConfig Instance
        {
            get
            {              
                return lazy.Value;
            }
        }
    }
}
