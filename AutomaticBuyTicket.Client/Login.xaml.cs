using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AutomaticBuyTicket
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        DispatcherTimer qrCodeTimer = new DispatcherTimer();

        public Login()
        {
            InitializeComponent();
            qrCodeTimer.Interval = new TimeSpan(0, 0, 1);   //时间间隔为一秒
            qrCodeTimer.Tick += new EventHandler(CheckQRCode);
        }

        /// <summary>
        /// 加载扫码登陆二维码
        /// </summary>
        public async void LoadQRCode()
        {
            await Core.Auth.Login.GetQRCodeAsync();

            string imagebase64 = Core.Auth.Login.QRCodeResult.image;
            byte[] streamBase = Convert.FromBase64String(imagebase64);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(streamBase);
            bi.EndInit();
            QRImg.Source = bi;

            qrCodeTimer.Start();

        }

        public async void CheckQRCode(object sender, EventArgs e)
        {
            await Core.Auth.Login.CheckQRCodeAsync();

            //扫码登录成功
            if (Core.Auth.Login.CheckQRCodeResult.result_code == "2")
            {
                ((DispatcherTimer)sender).Stop();

                await Core.Auth.Login.AuthAsync();
                await Core.Auth.Login.ClientAuthAsync();

            }

            //二维码过期
            if (Core.Auth.Login.CheckQRCodeResult.result_code == "3")
            {
                ((DispatcherTimer)sender).Stop();
                LoadQRCode();
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoginContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tabItem = LoginContainer.SelectedItem as TabItem;
            //扫码登录
            if (tabItem.Name == "LoginByQRCode")
            {
                LoadQRCode();               
            }

            //用户名密码登录
            if (tabItem.Name == "LoginByPassword")
            {
                qrCodeTimer.Stop();
            }

        }
    }
}
