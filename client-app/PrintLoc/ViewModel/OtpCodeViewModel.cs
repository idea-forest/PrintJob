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
    class OtpCodeViewModel : INotifyPropertyChanged
    {
        #region Properties

        private Window window;
        public ICommand ConfirmOtpCommand { get; set; }
        #endregion

        #region Constructor
        public OtpCodeViewModel(Window window)
        {
            this.window = window;
            ConfirmOtpCommand = new RelayCommand(ConfirmOtpCommandExecute);
        }
        #endregion

        #region Private Methods
        private async void ConfirmOtpCommandExecute()
        {
            if (OtpModel.Instance.Code == null)
            {
                MessageBox.Show("Please enter the otp code sent to your email");
                return;
            }

            var confirmOtp = await AccountManager.VerifyOtp(OtpModel.Instance.Code);
            if(confirmOtp != null)
            {
                if(confirmOtp.Status)
                {
                    MessageBox.Show("Authentication Confirmed");
                    string teamId = AuthResult.Instance.User.TeamId;
                    Device device = await AccountManager.StoreDevice(teamId);
                    if(device != null)
                    {
                        if(device.Passcode != null)
                        {
                            var homepageViewModel = new HomepageViewModel(window);
                            WindowManager.ChangeWindowContent(window, homepageViewModel, Resources.HomepageWindowTitle, Resources.HomepageControlPath);
                            if (homepageViewModel.CloseAction == null)
                            {
                                homepageViewModel.CloseAction = () => window.Close();
                            }
                        }else
                        {
                            window.Close();
                            EnablePinCodeWindow enablePinCode = new EnablePinCodeWindow();
                            enablePinCode.ShowDialog();
                        }
                    } else
                    {
                        MessageBox.Show("Something went wrong");
                        window.Close();
                    }
                }
                else
                {
                    MessageBox.Show(confirmOtp.Message);
                }
            } else
            {
                MessageBox.Show("Something went wrong");
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
