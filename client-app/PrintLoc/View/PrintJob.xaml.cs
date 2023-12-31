using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using PrintLoc.Model;

namespace PrintLoc.View
{
    /// <summary>
    /// Interaction logic for PrintJob.xaml
    /// </summary>
    public partial class PrintJob : UserControl
    {
        public ObservableCollection<PrintJobModel> PrintJobs { get; set; }

        public PrintJob()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
