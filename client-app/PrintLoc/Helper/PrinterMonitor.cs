using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Timers;
using PrintLoc.Model;
using System.Threading.Tasks;

namespace PrintLoc.Helper
{
    public class PrinterMonitor
    {
        private Timer printerCheckTimer;

        public class DetailedPrinterInfo
        {
            public string PrinterName { get; set; }
            public string PrinterType { get; set; }
            public string PrinterColor { get; set; }
            public string PrinterID { get; set; }
        }

        public void StartMonitoringPrinters()
        {
            printerCheckTimer = new Timer();
            printerCheckTimer.Interval = 5000;
            printerCheckTimer.Elapsed += CheckPrinters;
            printerCheckTimer.AutoReset = true;
            printerCheckTimer.Start();

            Console.WriteLine("Monitoring printers...");
        }

        private async void CheckPrinters(object sender, ElapsedEventArgs e)
        {
            string deviceId = ConnectedDevice.Instance.DeviceId;

            if (!string.IsNullOrEmpty(deviceId))
            {
                try
                {
                    List<string> printers = new List<string>();
                    foreach (string printer in PrinterSettings.InstalledPrinters)
                    {
                        printers.Add(printer);
                    }

                    await DisplayPrinterInformation(printers);
                    Console.WriteLine("Reading Printer");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving printer information: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("DeviceId is empty. Skipping printer check.");
            }
        }


        public async Task<List<DetailedPrinterInfo>> DisplayPrinterInformation(List<string> printers)
        {
            List<DetailedPrinterInfo> printerInfoList = new List<DetailedPrinterInfo>();
            string token = AuthResult.Instance.Token;
            string teamId = AuthResult.Instance.User.TeamId;
            string deviceId = ConnectedDevice.Instance.DeviceId;

            foreach (string printer in printers)
            {
                DetailedPrinterInfo printerInfo = new DetailedPrinterInfo();
                printerInfo.PrinterName = printer;
                PrinterSettings settings = new PrinterSettings { PrinterName = printer };
                printerInfo.PrinterColor = settings.SupportsColor ? "Color" : "Monochrome";
                printerInfoList.Add(printerInfo);
                await AccountManager.StoreDevicePrinter(deviceId, printerInfo.PrinterName, printerInfo.PrinterColor, teamId);
            }

            return printerInfoList;
        }

        public void StopMonitoringPrinters()
        {
            if (printerCheckTimer != null)
            {
                printerCheckTimer.Stop();
                printerCheckTimer.Dispose();
                Console.WriteLine("Stopped monitoring printers.");
            }
            else
            {
                Console.WriteLine("Printer monitoring is not running.");
            }
        }
    }
}
