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

namespace PrintLoc.View
{
    /// <summary>
    /// Interaction logic for ProfileSettings.xaml
    /// </summary>
    public partial class ProfileSettings : UserControl
    {
        public ProfileSettings()
        {
            InitializeComponent();
            Loaded += ProfileSettings_Loaded;
        }

        private void ProfileSettings_Loaded(object sender, RoutedEventArgs e)
        {
            Email.IsEnabled = false;
        }

        private void EditEmail_Click(object sender, RoutedEventArgs e)
        {
            Email.IsEnabled = true;
            Email.Focus();
        }


        private void UpdateProfile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
