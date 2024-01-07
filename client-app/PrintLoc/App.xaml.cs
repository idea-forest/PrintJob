using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;
using PrintLoc.Helper;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System;
using PrintLoc.View;

namespace PrintLoc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private static Mutex _mutex = null;
        private NotifyIcon _notifyIcon;
        private LaunchPage _mainWindow;

        public static bool IsUserAdministrator()
        {
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        private void NotifyIcon_DoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Show or bring the main window to the front when the icon is clicked
                if (_mainWindow != null)
                {
                    if (_mainWindow.Visibility != Visibility.Visible)
                    {
                        _mainWindow.Show();
                        _mainWindow.Activate();
                    }
                    else
                    {
                        _mainWindow.WindowState = WindowState.Normal;
                        _mainWindow.Activate();
                    }
                }
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "PrintLoc";
            bool createdNew;
            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                System.Windows.MessageBox.Show("Another instance of the application is already running.", "Application Running", MessageBoxButton.OK, MessageBoxImage.Warning);
                Current.Shutdown();
            } else
            {
                string releaseFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string iconFilePath = System.IO.Path.Combine(releaseFolder, "Icon.ico");

                _notifyIcon = new NotifyIcon();
                _notifyIcon.Icon = new System.Drawing.Icon(iconFilePath);
                _notifyIcon.Text = "Prinbloc";
                _notifyIcon.Visible = true;
                _notifyIcon.MouseClick += NotifyIcon_DoubleClick;

                _mainWindow = new LaunchPage();
                ContextMenu menu = new ContextMenu();
                MenuItem exitMenuItem = new MenuItem("Exit");
                exitMenuItem.Click += ExitMenuItem_Click;
                menu.MenuItems.Add(exitMenuItem);
                _notifyIcon.ContextMenu = menu;
                base.OnStartup(e);
                PrinterMonitor printerHelper = new PrinterMonitor();
                printerHelper.StartMonitoringPrinters();
                InternetConnectivityMonitor.StartMonitoring(5);

                //call signalr job
                SignalRBackgroundService signalBack = new SignalRBackgroundService();
                signalBack.StartSignalRConnectionInBackground();
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Current.Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon?.Dispose();
            _mutex?.Dispose();
            base.OnExit(e);
        }
    }
}
