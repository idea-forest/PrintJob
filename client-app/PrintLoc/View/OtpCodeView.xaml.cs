using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using PrintLoc.Model;
using PrintLoc.Helper;

namespace PrintLoc.View
{
    public partial class OtpCodeView : UserControl
    {
        private DispatcherTimer resendTimer;
        private int secondsRemaining = 120;
        private Hyperlink otpLink;

        public OtpCodeView()
        {
            InitializeComponent();
            InitializeResendTimer();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            OtpModel.Instance.Code = Code.Password;
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            Code.Password = null;
        }

        private void InitializeResendTimer()
        {
            resendTimer = new DispatcherTimer();
            resendTimer.Interval = TimeSpan.FromSeconds(1);
            resendTimer.Tick += ResendTimerTick;
        }

        private void ResendTimerTick(object sender, EventArgs e)
        {
            secondsRemaining--;

            if (secondsRemaining <= 0)
            {
                resendTimer.Stop();
                secondsRemaining = 120;
                if (otpLink != null)
                {
                    otpLink.Inlines.Clear();
                    otpLink.Inlines.Add(new Run("Resend OTP"));
                    otpLink.IsEnabled = true;
                }
            }
            else
            {
                if (otpLink != null)
                {
                    otpLink.Inlines.Clear();
                    otpLink.Inlines.Add(new Run($"You can resend in {secondsRemaining} seconds"));
                }
            }
        }

        private async void ResendOtpClick(object sender, RoutedEventArgs e)
        {
            otpLink = (Hyperlink)sender;
            if (otpLink != null)
            {
                otpLink.IsEnabled = false;
                PasscodeResponse resendOtp = await AccountManager.ResendOtp();
                if(resendOtp != null)
                {
                    if(resendOtp.Status)
                    {
                        MessageBox.Show(resendOtp.Message);
                    } else
                    {
                        MessageBox.Show(resendOtp.Message);
                    }
                } else
                {
                    MessageBox.Show("Opps! something went wrong");
                }
                resendTimer.Start();
            }
        }
    }
}
