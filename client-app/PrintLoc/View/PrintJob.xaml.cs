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
            PrintJobs = new ObservableCollection<PrintJobModel>();
            PrintJobs.Add(new PrintJobModel
            {
                Id = 1,
                FilePath = @"C:\Documents\Document1.pdf",
                Color = "Black and White",
                Page = "A4",
                Copies = "2",
                TeamId = 1,
                DeviceId = "Printer001",
                PrinterName = "Printer 1",
                UserId = "user123",
                Status = "In Progress",
                CreatedAt = DateTime.Now
            });

            PrintJobs.Add(new PrintJobModel
            {
                Id = 2,
                FilePath = @"C:\Documents\Document2.docx",
                Color = "Color",
                Page = "Letter",
                Copies = "1",
                TeamId = 2,
                DeviceId = "Printer002",
                PrinterName = "Printer 2",
                UserId = "user456",
                Status = "Completed",
                CreatedAt = DateTime.Now.AddDays(-1)
            });
            DataContext = this;
        }
    }
}
