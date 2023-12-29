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
    class PrinterListViewModel : INotifyPropertyChanged
    {
        #region Properties
        public Action CloseAction { get; set; }

        private Window window;

        public ICommand PrintListCommand { get; set; }
        #endregion

        #region Constructor
        public PrinterListViewModel(Window window)
        {
            this.window = window;
            PrintListCommand = new RelayCommand(PrintListCommandExecute);
        }
        #endregion

        #region Private Methods
        private void PrintListCommandExecute()
        {
            var printListViewModel = new PrinterListViewModel(window);
            WindowManager.ChangeWindowContent(window, printListViewModel, Resources.PrinterListWindowTitle, Resources.PrinterListControlPath);
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
