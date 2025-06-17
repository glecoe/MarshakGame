using System.Windows;
using System.Windows.Controls;

namespace MarshakGame.Views
{
    public partial class StoryWindow : Window
    {
        public StoryWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new StoryPage());
        }

        public void NavigateTo(Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}