using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using PrintLoc.Model;
using PrintLoc.View;
using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace PrintLoc.Helper
{
    public class SignalRBackgroundService
    {
        private static string apiUrl = ApiBaseUrl.BaseUrl;

        private HubConnection hubConnection;
        private CancellationTokenSource cancellationTokenSource;
        private string deviceId = ConnectedDevice.Instance.DeviceId;

        public async void StartSignalRConnectionInBackground()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(apiUrl + "printjob")
                .Build();

            hubConnection.On<PrintJobModel>("ReceivePrintJobs", async (job) =>
            {
                Console.WriteLine("retrieving device " + deviceId);
                if (job.DeviceId == deviceId && job.Status == "pending")
                {
                    Status.Instance.Online = true;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (job.IsPasscode)
                        {
                            //await SendToPrinter(job);
                            PassCodeWindow passCodeWindow = new PassCodeWindow(job);
                            passCodeWindow.ShowDialog();
                        }
                        else
                        {
                            ProcessingWindow processingWindow = new ProcessingWindow(job);
                            processingWindow.ShowDialog();
                        }
                    });
                }
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
                Status.Instance.Online = true;
                Console.WriteLine("SignalR connection started.");

                hubConnection.Closed += async (error) =>
                {
                    Status.Instance.Online = false;
                    Console.WriteLine($"SignalR connection closed: {error?.Message}");
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                    await ConnectWithRetry(cancellationToken);
                };
            }
            catch (Exception ex)
            {
                Status.Instance.Online = false;
                Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                await ConnectWithRetry(cancellationToken);
            }
        }

        public async Task<bool> SendToPrinter(PrintJobModel job)
        {
            try
            {
                String Status = "processing";
                String Message = $"Processing your print";
                await AccountManager.updatePrintJob(job.Id, Status, Message, 0, "None");

                Console.WriteLine("sending file");
                Print printHelper = new Print();
                int JobId = job.Id;
                string fileUrl = job.FilePath;
                //string fileUrl = "";
                string printerName = job.PrinterName;
                bool isColor = job.Color;
                int startPage = job.StartPage;
                int endPage = job.EndPage;
                int numberOfCopies = job.Copies;
                string paperName = job.PaperName;
                bool landScape = job.LandScape;
                await printHelper.PrintFileFromUrl(fileUrl, printerName, paperName, isColor, startPage, endPage, landScape, numberOfCopies, JobId);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ProcessingWindow processingWindow = new ProcessingWindow();
                    processingWindow.Show();
                    processingWindow.Close();
                });
                return true;
            }
            catch(Exception ex)
            {
                String Status = "failed";
                String Message = $"Error printing your document: {ex.Message}";
                await AccountManager.updatePrintJob(job.Id, Status, Message, 0, "None");

                Application.Current.Dispatcher.Invoke(() =>
                {
   
                    PrintFailureWindow printFailureWindow = new PrintFailureWindow();
                    printFailureWindow.ShowDialog();
                });
                return false;
            }
        }
    }
}
