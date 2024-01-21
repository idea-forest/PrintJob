using System;
using System.Threading.Tasks;
using System.Windows;
using PrintLoc.Model;
using PrintLoc.Helper;

namespace PrintLoc.View
{
    /// <summary>
    /// Interaction logic for ProcessingWindow.xaml
    /// </summary>
    public partial class ProcessingWindow : Window
    {
        private PrintJobModel printJob;

        public ProcessingWindow(PrintJobModel printJob = null)
        {
            InitializeComponent();
            this.printJob = printJob;
            Loaded += MainWindow_Loaded;
            Closing += WindowClosing;
            WindowStartupLocation = WindowStartupLocation.Manual;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetWindowPositionAboveTaskbar();
            Activate();

            await ProcessPrintJobAsync();
        }

        private async Task ProcessPrintJobAsync()
        {
            await Task.Run(async () =>
            {
                SignalRBackgroundService signalRBackgroundService = new SignalRBackgroundService();
                await signalRBackgroundService.SendToPrinter(printJob);

                string status = "processing";
                string message = "Processing your print";

                // Assuming AccountManager.updatePrintJob is also an asynchronous operation
                await AccountManager.updatePrintJob(printJob.Id, status, message, 0, "None");
            });
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void SetWindowPositionAboveTaskbar()
        {
            double screenWidth = SystemParameters.WorkArea.Width;
            double screenHeight = SystemParameters.WorkArea.Height;
            double taskbarHeight = SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Height;
            Left = screenWidth - Width; // Adjust as needed
            Top = screenHeight - taskbarHeight - Height - 10;
        }
    }
}
