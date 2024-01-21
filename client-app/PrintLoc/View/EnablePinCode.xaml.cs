using System;
using System.Windows;
using System.Windows.Controls;
using PrintLoc.Model;
using PrintLoc.Helper;

namespace PrintLoc.View
{
    /// <summary>
    /// Interaction logic for EnablePinCode.xaml
    /// </summary>
    public partial class EnablePinCode : UserControl
    {
        private bool isPinCodeValid = false;

        public EnablePinCode()
        {
            InitializeComponent();
            Loaded += PincodeSettings_Loaded;
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnablePincodeInstance.Instance.ConfirmPincode = ConfirmPincode.Password;
            EnablePincodeInstance.Instance.NewPincode = NewPincode.Password;
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            ConfirmPincode.Password = null;
            NewPincode.Password = null;
        }

        private void PincodeSettings_Loaded(object sender, RoutedEventArgs e)
        {
            isPinCodeValid = false;
            UpdateUI();
        }

        private void Pincode_Click(object sender, RoutedEventArgs e)
        {
            isPinCodeValid = true;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (isPinCodeValid)
            {
                NewPincode.Visibility = Visibility.Collapsed;
                ConfirmPincode.Visibility = Visibility.Collapsed;
            }
            else
            {
                NewPincode.Visibility = Visibility.Visible;
                ConfirmPincode.Visibility = Visibility.Visible;
            }
        }
    }
}
