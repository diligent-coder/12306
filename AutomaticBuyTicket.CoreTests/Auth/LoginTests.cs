using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomaticBuyTicket.Core.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomaticBuyTicket.Core.Auth.Tests
{
    [TestClass()]
    public class LoginTests
    {
        [TestMethod()]
        public void GetQRCodeAsyncTest()
        {
            var result=Login.GetQRCodeAsync();
        }
    }
}