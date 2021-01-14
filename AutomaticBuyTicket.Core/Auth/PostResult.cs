namespace AutomaticBuyTicket.Core.Auth
{
    public class CommonResult
    {
        public string result_message { get; set; }
        public string result_code { get; set; }
    }

    public class CreateQRCodeResult : CommonResult
    {
        public string image { get; set; }

        public string uuid { get; set; }
    }

    public class CheckQRCodeResult : CommonResult
    {
        public string uamtk { get; set; }
    }

    public class AuthResult : CommonResult
    {
        public string apptk { get; set; }

        public string newapptk { get; set; }
    }

    public class ClientAuthResult : CommonResult
    {
        public string apptk { get; set; }

        public string username { get; set; }
    }
}
