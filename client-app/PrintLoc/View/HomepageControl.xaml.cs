using System;
using Microsoft.AspNetCore.SignalR.Client;
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
        private HubConnection hubConnection;
        private string apiUrl = ApiBaseUrl.BaseUrl;
        private string deviceId = ConnectedDevice.Instance.DeviceId;

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
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(apiUrl + "checkConnection")
                    .Build();

                hubConnection.Closed += async (error) =>
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await ConnectToHub();
                };
                await ConnectToHub();
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

        private async void InitializeAsync()
        {
            string userName = AuthResult.Instance.User?.UserName;
            string teamName = AuthResult.Instance.User?.TeamId;
            string token = AuthResult.Instance.Token;
            string teamId = AuthResult.Instance.User.TeamId;
            int width = 200;
            int height = 200;

            BitmapImage qrCodeBitmap = Qrcode.GenerateQRCode(teamName, width, height);
            DeviceIDText.Text = $"Device ID: {deviceId}";
            QRCodeImage.Source = qrCodeBitmap;

            Print printHelper = new Print();
            string fileUrl = "https://www.africau.edu/images/default/sample.pdf";
            string printerName = "Microsoft Print to PDF";
            bool isColor = true;
            int startPage = 1;
            int endPage = 0;
            int numberOfCopies = 1;
            bool printStatus = await printHelper.PrintFileFromUrl(fileUrl, printerName, isColor, startPage, endPage, numberOfCopies);
            if (printStatus)
            {
                Console.WriteLine("File printed successfully.");
            }
            else
            {
                Console.WriteLine("Failed to print the file.");
            }
        }

        private async Task ConnectToHub()
        {
            try
            {
                await hubConnection.StartAsync();
                await SendDeviceId(deviceId);
                StartSendingHeartbeat();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to hub: {ex.Message}");
                // Handle connection error
            }
        }

        private async Task SendDeviceId(string deviceId)
        {
            try
            {
                // Invoke the Heartbeat method on the server and pass the device ID
                await hubConnection.InvokeAsync("ReceiveHeartbeat", deviceId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending device ID: {ex.Message}");
                // Handle error while sending device ID
            }
        }

        private async void StartSendingHeartbeat()
        {
            while (true)
            {
                try
                {
                    await SendDeviceId(deviceId);
                    await Task.Delay(TimeSpan.FromSeconds(2)); // Adjust heartbeat interval here
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending heartbeat: {ex.Message}");
                }
            }
        }
    }
}
