using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PrintLoc.Helper;
using PrintLoc.Model;
using PrintLoc.ViewModel;

namespace PrintLoc.View
{
    /// <summary>
    /// Interaction logic for PrinterListView.xaml
    /// </summary>
    public partial class PrinterListView : Window
    {
        public ObservableCollection<Printer> Printers { get; set; }
        private ICollectionView _collectionView;

        public PrinterListView()
        {
            InitializeComponent();
            Printers = new ObservableCollection<Printer>();
            _collectionView = CollectionViewSource.GetDefaultView(Printers);
            _collectionView.Filter = Filter;
            Loaded += async (_, __) => await LoadPrintersAsync();
            DataContext = new PrinterListViewModel(this);
        }

        private async Task LoadPrintersAsync()
        {
            try
            {
                string deviceId = DeviceIdManager.GetDeviceId();
                string token = AuthResult.Instance.Token;
                Printer printer = await AccountManager.getAllPrinters(deviceId, token);
                Printers = new ObservableCollection<Printer>();
                if (printer != null)
                {
                    Printers.Add(printer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading printers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_collectionView != null)
            {
                _collectionView.Refresh();
            }
        }

        private bool Filter(object obj)
        {
            if (obj is Printer printer)
            {

            }
            return false;
        }
    }
}
