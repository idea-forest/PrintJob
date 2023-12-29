using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PrintLoc.Model;
using PrintLoc.ViewModel;

namespace PrintLoc.View
{
    /// <summary>
    /// Interaction logic for PrintJobView.xaml
    /// </summary>
    public partial class PrintJobView : Window
    {
        public PrintJobView()
        {
            InitializeComponent();
            DataContext = new PrintJobViewModel(this);
        }
    }
}
