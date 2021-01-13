using AutomaticBuyTicket.Core.Http.ContentTypeAlgorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;

namespace AutomaticBuyTicket.Core.Http.Tests
{
    [TestClass]
    public class HttpContentFactoryTests
    {
        [TestMethod()]
        public void GetContentProccesserTest()
        {
            var proccesser = HttpContentFactory.GetContentProccesser("application/json");
            Assert.IsTrue(proccesser is StringContentProccesser);
        }
    }
}