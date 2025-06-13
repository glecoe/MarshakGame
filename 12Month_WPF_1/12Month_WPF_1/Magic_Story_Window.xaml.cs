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
    /// Interaction logic for Magic_Story_Window.xaml
    /// </summary>
    public partial class Magic_Story_Window : Window
    {
        public static Magic_Story_Window _Magic_Story;
        public Magic_Story_Window()
        {
            InitializeComponent();
            _Magic_Story = this;
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
