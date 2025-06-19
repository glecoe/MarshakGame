using MarshakGame.Properties;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

namespace MarshakGame.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            // При старте — показываем страницу меню
            MainFrame.Navigate(new MenuPage());
        }
        public void NavigateTo(Page page)
        {
            MainFrame.Navigate(page);
        }
        private void LoadSettings()
        {
            // Применяем настройки при запуске
            if (Settings.Default.Fullscreen)
            {
                WindowState = WindowState.Maximized;
                WindowStyle = WindowStyle.None;
            }
            else
            {
                Width = Settings.Default.ResolutionWidth;
                Height = Settings.Default.ResolutionHeight;
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.SingleBorderWindow;
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
