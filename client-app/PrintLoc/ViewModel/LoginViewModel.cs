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
    class LoginViewModel : INotifyPropertyChanged
    {
        #region Properties

        private Window window;

        public ICommand LoginCommand { get; set; }

        public Action CloseAction { get; set; }

        private string _email;

        private string _teamName;

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email == value) return;
                _email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
            }
        }

        public string TeamName
        {
            get { return _teamName; }
            set
            {
                if (_teamName == value) return;
                _teamName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TeamName"));
            }
        }

        #endregion

        #region Constructor
        public LoginViewModel(Window window)
        {
            this.window = window;
            UserModel.Instance.Email = Email;
            UserModel.Instance.TeamName = TeamName;
            LoginCommand = new RelayCommand(LoginCommandExecute);
        }
        #endregion

        #region Private Methods
        private async void LoginCommandExecute()
        {
            UserModel.Instance.Email = Email;
            UserModel.Instance.TeamName = TeamName;
            if (UserModel.Instance.TeamName == null)
            {
                MessageBox.Show("Teamname is required");
                return;
            }

            if (UserModel.Instance.Email == null)
            {
                MessageBox.Show("EMail address is required");
                return;
            }

            if(UserModel.Instance.Password == null)
            {
                MessageBox.Show("Password is required");
                return;
            }

            var loggedInUser = await AccountManager.LoginAccount(UserModel.Instance.TeamName, UserModel.Instance.Email, UserModel.Instance.Password);
            if(loggedInUser.Success)
            {
                if(loggedInUser.User.EmailConfirmed)
                {
                    window.Close();
                    OtpCodeWindow otpCodeWindow = new OtpCodeWindow();
                    otpCodeWindow.ShowDialog();
                } else
                {
                    MessageBox.Show("Please verify your account on the web to continue using prinbloc");
                }
            } else
            {
                MessageBox.Show(loggedInUser.Errors.ToString());
            }
            //if (loggedInUser != null)
            //{
            //    var homepageViewModel = new HomepageViewModel(window);
            //    WindowManager.ChangeWindowContent(window, homepageViewModel, Resources.HomepageWindowTitle, Resources.HomepageControlPath);
            //    if (homepageViewModel.CloseAction == null)
            //    {
            //        homepageViewModel.CloseAction = () => window.Close();
            //    }
            //}
            //else
            //{
            //   MessageBox.Show("Invalid credentials.");
            //}
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
