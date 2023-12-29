using PrintLoc.Command;
using PrintLoc.Helper;
using PrintLoc.Model;
using PrintLoc.Properties;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PrintLoc.ViewModel
{
    class PrintJobViewModel : INotifyPropertyChanged
    {
        #region Properties
        public Action CloseAction { get; set; }

        private Window window;

        public ICommand PrintJobCommand { get; set; }
    #endregion

        #region Constructor
        public PrintJobViewModel(Window window)
        {
            this.window = window;
            PrintJobCommand = new RelayCommand(PrintJobCommandExecute);
        }
    #endregion

        #region Private Methods
        private void PrintJobCommandExecute()
        {
            var printJobViewModel = new PrintJobViewModel(window);
            WindowManager.ChangeWindowContent(window, printJobViewModel, Resources.PrintJobWindowTitle, Resources.PrintJobControlPath);
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}