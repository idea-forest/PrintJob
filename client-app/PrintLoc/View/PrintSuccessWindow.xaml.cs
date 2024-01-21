using System.Windows;
using NAudio.Wave;
using System.IO;

namespace PrintLoc.View
{
    /// <summary>
    /// Interaction logic for PrintSuccessWindow.xaml
    /// </summary>
    public partial class PrintSuccessWindow : Window
    {
        private IWavePlayer wavePlayer;
        private AudioFileReader audioFileReader;

        public PrintSuccessWindow()
        {
            InitializeComponent();
            //wavePlayer = new WaveOut();
            //string FilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Resources\\success-1-6297.mp3");
            //audioFileReader = new AudioFileReader(FilePath);
            //if (wavePlayer is WaveOut waveOut)
            //{
            //    waveOut.Volume = 0.5f;
            //}
            Loaded += MainWindow_Loaded;
            WindowStartupLocation = WindowStartupLocation.Manual;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
