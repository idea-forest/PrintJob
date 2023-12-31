using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PrintLoc.Helper;

namespace PrintLoc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            PrinterMonitor printerHelper = new PrinterMonitor();
            printerHelper.StartMonitoringPrinters();
            InternetConnectivityMonitor.StartMonitoring(5);

            //call signalr job
            SignalRBackgroundService signalBack= new SignalRBackgroundService();
            signalBack.StartSignalRConnectionInBackground();
        }

    }
}
