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
    /// Interaction logic for FloatingActionWindows.xaml
    /// </summary>
    public partial class FloatingActionWindows : UserControl
    {
        public FloatingActionWindows()
        {
            InitializeComponent();
            fabButton.Click += FabButton_Click;
        }

        private void FabButton_Click(object sender, RoutedEventArgs e)
        {
            fabMenu.IsOpen = !fabMenu.IsOpen;
        }

        private void ProfileSetting_Click(object sender, RoutedEventArgs e)
        {
            ProfileSettingsWindow profileSettingsWindow = new ProfileSettingsWindow();
            profileSettingsWindow.ShowDialog();
            fabMenu.IsOpen = false;
        }

        private void PinSetting_Click(object sender, RoutedEventArgs e)
        {
            EnablePinCodeWindow enablePinCode = new EnablePinCodeWindow();
            enablePinCode.ShowDialog();
            fabMenu.IsOpen = false;
        }
    }
}
