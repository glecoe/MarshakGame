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
using System.Windows.Shapes;

namespace _12Month_WPF_1
{
    /// <summary>
    /// Interaction logic for Settings_Window.xaml
    /// </summary>
    public partial class Settings_Window : Window
    {
        public static Settings_Window _Settings_Window;
        public Settings_Window()
        {
            InitializeComponent();
            _Settings_Window = this;
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Main_Window = new MainWindow();
            this.Close();
            Main_Window.Show();
        }
    }
}
