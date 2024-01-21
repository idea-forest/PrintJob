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
using System.Windows.Shapes;
using PrintLoc.Model;
using PrintLoc.ViewModel;

namespace PrintLoc.View
{
    /// <summary>
    /// Interaction logic for PassCodeWindow.xaml
    /// </summary>
    public partial class PassCodeWindow : Window
    {
        public PassCodeWindow(PrintJobModel job)
        {
            InitializeComponent();
            DataContext = new PasscodeWindowModel(this, job);
            Loaded += MainWindow_Loaded;
            WindowStartupLocation = WindowStartupLocation.Manual;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetWindowPositionAboveTaskbar();
            Activate();
        }

        private void SetWindowPositionAboveTaskbar()
        {
            double screenWidth = SystemParameters.WorkArea.Width;
            double screenHeight = SystemParameters.WorkArea.Height;
            double taskbarHeight = SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Height;
            Left = screenWidth - Width; // Adjust as needed
            Top = screenHeight - taskbarHeight - Height - 10;
        }
    }
}
