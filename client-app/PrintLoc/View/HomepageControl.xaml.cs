using System;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using PrintLoc.Helper;
using PrintLoc.Model;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Windows.Media;
using System.Timers;
using System.Windows.Shapes;

namespace PrintLoc.View
{
    /// <summary>
    /// Interaction logic for HomepageControl.xaml
    /// </summary>
    public partial class HomepageControl : UserControl
    {
        private string deviceId = DeviceIdManager.GetDeviceId();

        private Timer connectivityCheckTimer;
        public HomepageControl()
        {
            InitializeComponent();
            InitializeAsync();
            OnInternetConnectivityChanged();
            StartInternetConnectivityCheck();
        }

        private void StartInternetConnectivityCheck()
        {
            connectivityCheckTimer = new Timer();
            connectivityCheckTimer.Interval = 2000;
            connectivityCheckTimer.Elapsed += OnTimerElapsed;
            connectivityCheckTimer.AutoReset = true;
            connectivityCheckTimer.Enabled = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            OnInternetConnectivityChanged();
        }


        private async void OnInternetConnectivityChanged()
        {
            bool isConnected = Status.Instance.Online;
            if (isConnected)
            {
                SetDeviceStatus("Online");
                SetEllipseColor(Colors.Green);
            }
            else
            {
                SetDeviceStatus("Offline");
                SetEllipseColor(Colors.Red);
                Console.WriteLine("internet disconnected");
            }
        }

        private void SetDeviceStatus(string text)
        {
            Dispatcher.Invoke(() =>
            {
                DeviceStatusText.Text = text;
            });
        }


        private void SetEllipseColor(Color color)
        {
            Dispatcher.Invoke(() =>
            {
                ConnectivityStatusEllipse.Fill = new SolidColorBrush(color);
            });
        }

        private Task InitializeAsync()
        {
            int width = 200;
            int height = 200;

            BitmapImage qrCodeBitmap = Qrcode.GenerateQRCode(deviceId, width, height);
            DeviceIDText.Text = $"Device ID: {deviceId}";
            QRCodeImage.Source = qrCodeBitmap;
            return Task.CompletedTask;
        }
    }
}
