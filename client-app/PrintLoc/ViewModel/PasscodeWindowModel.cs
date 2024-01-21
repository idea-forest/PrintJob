using PrintLoc.Model;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using PrintLoc.Helper;
using PrintLoc.Properties;
using System.Net.Mail;
using System;
using PrintLoc.Command;
using System.IO;
using System.Threading.Tasks;
using PrintLoc.View;

namespace PrintLoc.ViewModel
{
    class PasscodeWindowModel : INotifyPropertyChanged
    {
        #region Properties
        private Window window;
        private PrintJobModel printJob;
        public ICommand PasscodeWindowModelCommand { get; set; }
        private string deviceId = DeviceIdManager.GetDeviceId();
        public string DataToPass { get; private set; }
        #endregion

        #region Constructor
        public PasscodeWindowModel(Window window, PrintJobModel job)
        {
            this.window = window;
            this.printJob = job;
            PasscodeWindowModelCommand = new RelayCommand(PasscodeWindowModelCommandExecute);
            this.window.Closing += WindowClosing;
        }
        #endregion

        #region Private Methods
        private async void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (e.Cancel)
            {
                String Status = "failed";
                String Message = $"Error printing cancelled";
                await AccountManager.updatePrintJob(printJob.Id, Status, Message, 0, "None");
                MessageBox.Show("You cancelled the print process");
            }
        }
        #endregion

        #region Private Methods
        private async void PasscodeWindowModelCommandExecute()
        {
            if(PasscodeModel.Instance.Passcode == null)
            {
                MessageBox.Show("Please enter your passcode to proceed with printing");
                return;
            }
            else if(PasscodeModel.Instance.Passcode != printJob.Passcode)
            {
                MessageBox.Show("Passcode is incorrect");
                return;
            } 
            else
            {
                window.Close();
                try
                {
                    ProcessingWindow processingWindow = new ProcessingWindow(printJob);
                    processingWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    String Status = "failed";
                    String Message = $"Error printing your document: {ex.Message}";
                    await AccountManager.updatePrintJob(printJob.Id, Status, Message, 0, "None");
                    MessageBox.Show("Error processing your print " + ex.Message);
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
