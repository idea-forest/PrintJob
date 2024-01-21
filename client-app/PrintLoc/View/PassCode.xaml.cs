using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PrintLoc.Model;

namespace PrintLoc.View
{
    /// <summary>
    /// Interaction logic for PassCode.xaml
    /// </summary>
    public partial class PassCode : UserControl
    {
        public PassCode()
        {
            InitializeComponent();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasscodeModel.Instance.Passcode = Passcode.Password;
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            Passcode.Password = null;
        }
    }
}
