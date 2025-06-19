using System.Windows;
using System.Windows.Controls;
using MarshakGame.Properties;
using System.Collections.Generic;
using System.Linq;

namespace MarshakGame.Views
{
    public partial class SettingsWindow : Window
    {
        public class ResolutionItem
        {
            public string Name { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public SettingsWindow()
        {
            InitializeComponent();
            LoadResolutions();
            DataContext = Settings.Default;
        }

        private void LoadResolutions()
        {
            var resolutions = new List<ResolutionItem>
            {
                new ResolutionItem { Name = "1920x1080", Width = 1920, Height = 1080 },
                new ResolutionItem { Name = "1600x900", Width = 1600, Height = 900 },
                new ResolutionItem { Name = "1366x768", Width = 1366, Height = 768 },
                new ResolutionItem { Name = "1280x720", Width = 1280, Height = 720 }
            };

            ResolutionComboBox.ItemsSource = resolutions;

            // Устанавливаем текущее разрешение
            var current = resolutions.FirstOrDefault(r =>
                r.Width == Settings.Default.ResolutionWidth &&
                r.Height == Settings.Default.ResolutionHeight);

            ResolutionComboBox.SelectedItem = current ?? resolutions[0];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Позиционируем окно в правом верхнем углу
            var mainWindow = Application.Current.MainWindow;
            Left = mainWindow.Left + mainWindow.Width - Width - 10;
            Top = mainWindow.Top + 10;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
            Close();
        }

        private void ReturnToMenu_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new MenuPage());
            }
            Close();
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}