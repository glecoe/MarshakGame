using System.Windows;
using System.Windows.Controls;

namespace MarshakGame.Views
{
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow main)
            {
                main.NavigateTo(new GamePage());
            }
        }

        private void ShowWorks_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) as MainWindow;
            if (window != null)
            {
                window.NavigateTo(new MagicWorksPage());
            }
        }

        private void AboutMarshak_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow main)
            {
                main.NavigateTo(new AboutMarshakPage());
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
