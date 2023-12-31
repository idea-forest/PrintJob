using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using PrintLoc.Model;
using Newtonsoft.Json;
using System.IO;

namespace PrintLoc.Helper
{
    public class SignalRBackgroundService
    {
        private static string apiUrl = ApiBaseUrl.BaseUrl;

        private HubConnection hubConnection;
        private CancellationTokenSource cancellationTokenSource;
        private string deviceId = DeviceIdManager.GetDeviceId();

        public async void StartSignalRConnectionInBackground()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(apiUrl + "printjob")
                .Build();

            hubConnection.On<PrintJobModel>("ReceivePrintJobs", async (job) =>
            {
                Console.WriteLine(job);
                await SendToPrinter(job);
                //if (job.DeviceId == deviceId && job.Status == "Pending")
                //{
                //    await SendToPrinter(job);
                //}
            });

            cancellationTokenSource = new CancellationTokenSource();
            await ConnectWithRetry(cancellationTokenSource.Token);
        }

        public void StopSignalRConnection()
        {
            cancellationTokenSource?.Cancel();
            hubConnection?.StopAsync().Wait();
            hubConnection?.DisposeAsync();
        }

        private async Task ConnectWithRetry(CancellationToken cancellationToken)
        {
            try
            {
                await hubConnection.StartAsync(cancellationToken);
                Console.WriteLine("SignalR connection started.");

                hubConnection.Closed += async (error) =>
                {
                    Console.WriteLine($"SignalR connection closed: {error?.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                    await ConnectWithRetry(cancellationToken);
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                await ConnectWithRetry(cancellationToken);
            }
        }

        private async Task<bool> SendToPrinter(PrintJobModel job)
        {
            Print printHelper = new Print();
            int JobId = job.Id;
            string fileUrl = job.FilePath;
            string printerName = job.PrinterName;
            bool isColor = job.Color;
            int startPage = job.StartPage;
            int endPage = job.EndPage;
            int numberOfCopies = job.Copies;
            string paperName = job.PaperName;
            bool landScape = job.LandScape;
            await printHelper.PrintFileFromUrl(fileUrl, printerName, paperName, isColor, startPage, endPage, landScape, numberOfCopies, JobId);
            return true;
        }
    }
}
