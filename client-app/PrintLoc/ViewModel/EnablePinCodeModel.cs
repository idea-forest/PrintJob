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
    class EnablePinCodeModel : INotifyPropertyChanged
    {
        #region Properties

        private Window window;
        public ICommand EnablePinCodeModelCommand { get; set; }
        private string deviceId = DeviceIdManager.GetDeviceId();
        #endregion

        #region Constructor
        public EnablePinCodeModel(Window window)
        {
            this.window = window;
            EnablePinCodeModelCommand = new RelayCommand(EnablePinCodeModelCommandExecute);
        }
        #endregion

        #region Private Methods
        private async void EnablePinCodeModelCommandExecute()
        {
            if (EnablePincodeInstance.Instance.NewPincode == null)
            {
                MessageBox.Show("Please enter your pincode");
                return;
            }

            if(EnablePincodeInstance.Instance.ConfirmPincode == null)
            {
                MessageBox.Show("Please enter confirm pincode");
                return;
            }

            if (EnablePincodeInstance.Instance.NewPincode.ToLower() != EnablePincodeInstance.Instance.ConfirmPincode.ToLower())
            {
                MessageBox.Show("Pincode and confirm pincode do not match");
                return;
            }

            Console.WriteLine("Token Plus" + deviceId, EnablePincodeInstance.Instance.NewPincode);
            PasscodeResponse pinCode = await AccountManager.EnablePinCode(deviceId, EnablePincodeInstance.Instance.NewPincode);
            if(pinCode != null)
            {
                if(pinCode.Status)
                {
                    MessageBox.Show(pinCode.Message);
                    var homepageViewModel = new HomepageViewModel(window);
                    WindowManager.ChangeWindowContent(window, homepageViewModel, Resources.HomepageWindowTitle, Resources.HomepageControlPath);
                    if (homepageViewModel.CloseAction == null)
                    {
                        homepageViewModel.CloseAction = () => window.Close();
                    }
                } else
                {
                    window.Close();
                    MessageBox.Show(pinCode.Message);
                }
            } else
            {
                window.Close();
                MessageBox.Show("Something went wrong");
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
