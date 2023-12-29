using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PrintLoc.Model;

namespace PrintLoc.Helper
{
    public class SendPrintJob
    {
        private static string apiUrl = ApiBaseUrl.BaseUrl;

        public async Task StartPrintJobListener(string deviceId)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl(apiUrl + "printjob")
                .Build();

            hubConnection.On<PrintJobModel>("PrintJobReady", async (job) =>
            {
                if (job.DeviceId == deviceId && job.Status == "Pending")
                {
                    bool success = await SendToPrinter(job);
                    if (success)
                    {
                    }
                    else
                    {

                    }
                }
            });

            try
            {
                await hubConnection.StartAsync();
                Console.WriteLine("SignalR connection started.");

                // Listen for incoming print job notifications
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
                // Handle connection start error
            }
        }

        private async Task<bool> SendToPrinter(PrintJobModel job)
        {
            return true;
        }
    }
}
