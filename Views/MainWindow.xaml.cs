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

            // При старте — показываем страницу меню
            MainFrame.Navigate(new MenuPage());
        }
        public void NavigateTo(Page page)
        {
            MainFrame.Navigate(page);
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            // Открыть окно настроек (можно и как модалку, а можно навигировать в Frame)
            var settings = new SettingsWindow();
            settings.Owner = this;
            settings.ShowDialog();
        }
    }
}
