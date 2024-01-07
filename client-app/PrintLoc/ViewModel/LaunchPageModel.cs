using PrintLoc.Command;
using PrintLoc.Helper;
using PrintLoc.Model;
using PrintLoc.Properties;
using System;
using System.ComponentModel;
using System.Windows;

namespace PrintLoc.ViewModel
{
    class LaunchPageModel : INotifyPropertyChanged
    {
        #region Properties
        private Window window;
        private string deviceId = DeviceIdManager.GetDeviceId();
        #endregion

        #region Constructor
        public LaunchPageModel(Window window)
        {
            this.window = window;

            if(deviceId == null)
            {
                var loginViewModel = new LoginViewModel(window);
                WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);
            } else
            {
                var homepageViewModel = new HomepageViewModel(window);
                WindowManager.ChangeWindowContent(window, homepageViewModel, Resources.HomepageWindowTitle, Resources.HomepageControlPath);
                if (homepageViewModel.CloseAction == null)
                {
                    homepageViewModel.CloseAction = () => window.Close();
                }
            }
        }
    #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
