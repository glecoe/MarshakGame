using MarshakGame.Properties;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

namespace MarshakGame.Views
{
    public partial class MainWindow : Window
    {
        private const int HWND_TOPMOST = -1;
        private const int SWP_SHOWWINDOW = 0x0040;
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter,
                                      int X, int Y, int cx, int cy, uint uFlags);
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) => {
                if (Settings.Default.Fullscreen)
                {
                    // Обновляем состояние после загрузки
                    var hwnd = new WindowInteropHelper(this).Handle;
                    SetWindowPos(hwnd, HWND_TOPMOST, 0, 0,
                                (int)SystemParameters.PrimaryScreenWidth,
                                (int)SystemParameters.PrimaryScreenHeight,
                                SWP_SHOWWINDOW);
                }
            };
            LoadSettings();
            MainFrame.Navigate(new MenuPage());
        }
        public void NavigateTo(Page page)
        {
            MainFrame.Navigate(page);
        }
        private void LoadSettings()
        {
            if (Settings.Default.Fullscreen)
            {
                WindowState = WindowState.Normal; // Важно сначала сбросить
                WindowStyle = WindowStyle.None;
                ResizeMode = ResizeMode.NoResize;
                WindowState = WindowState.Maximized;
                Topmost = true; // Дополнительная страховка
            }
            else
            {
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.SingleBorderWindow;
                ResizeMode = ResizeMode.CanResize;
                Topmost = false;
                Width = Settings.Default.ResolutionWidth;
                Height = Settings.Default.ResolutionHeight;
            }
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsWindow();
            settings.Closed += (s, args) => {
                // Применяем изменения после закрытия настроек
                LoadSettings();
            };
            settings.Owner = this;
            settings.ShowDialog();
        }
    }
}
