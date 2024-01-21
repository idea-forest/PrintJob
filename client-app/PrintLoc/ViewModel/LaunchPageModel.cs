using PrintLoc.Command;
using PrintLoc.Helper;
using PrintLoc.Model;
using PrintLoc.Properties;
using System;
using System.ComponentModel;
using System.Windows;
using PrintLoc.Model;
using PrintLoc.View;
using PrintLoc.Helper;

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
            LaunchAsync();
        }
        #endregion

        #region Private Methods
        private async void LaunchAsync()
        {
            if (deviceId == null)
            {
                var loginViewModel = new LoginViewModel(window);
                WindowManager.ChangeWindowContent(window, loginViewModel, Resources.LoginWindowTitle, Resources.LoginControlPath);
            }
            else
            {
                var homepageViewModel = new HomepageViewModel(window);
                WindowManager.ChangeWindowContent(window, homepageViewModel, Resources.HomepageWindowTitle, Resources.HomepageControlPath);
                if (homepageViewModel.CloseAction == null)
                {
                    homepageViewModel.CloseAction = () => window.Close();
                }
                //Device device = await AccountManager.StoreDevice();
                //if (device != null)
                //{
                //    if (device.Passcode != null)
                //    {
                //        var homepageViewModel = new HomepageViewModel(window);
                //        WindowManager.ChangeWindowContent(window, homepageViewModel, Resources.HomepageWindowTitle, Resources.HomepageControlPath);
                //        if (homepageViewModel.CloseAction == null)
                //        {
                //            homepageViewModel.CloseAction = () => window.Close();
                //        }
                //    }
                //    else
                //    {
                //        window.Close();
                //        EnablePinCodeWindow enablePinCode = new EnablePinCodeWindow();
                //        enablePinCode.ShowDialog();
                //    }
                //}
                //else
                //{
                //    window.Close();
                //    MessageBox.Show("Something went wrong");
                //}
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
