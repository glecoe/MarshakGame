using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _12Month_WPF_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static MainWindow Main_Window;
        public MainWindow()
        {
            InitializeComponent();
            Main_Window = this;
        }
        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            Game_Window _game_window = new Game_Window();
            _game_window.Show();
            Main_Window.Hide();
        }
        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            Settings_Window _Settings_Window = new Settings_Window();
            _Settings_Window.Show();
            Main_Window.Hide();
        }
        private void Magic_Story_Button_Click(object sender, RoutedEventArgs e)
        {
            Magic_Story_Window _Magic_Story = new Magic_Story_Window();
            _Magic_Story.Show();
            Main_Window.Hide();
        }
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }
    }
}
