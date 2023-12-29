using PrintLoc.Model;
using PrintLoc.ViewModel;
using System.Windows;

namespace PrintLoc.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel(this);
        }
    }
}
