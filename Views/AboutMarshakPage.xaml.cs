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

namespace MarshakGame.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutMarshakPage.xaml
    /// </summary>
    public partial class AboutMarshakPage : Page
    {
        public AboutMarshakPage()
        {
            InitializeComponent();
        }
        private void ReturnToMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MenuPage());
        }
    }
}
