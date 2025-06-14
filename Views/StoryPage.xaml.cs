using MarshakGame.Models;
using MarshakGame.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MarshakGame.Views
{
    public partial class StoryPage : Page
    {
        private readonly StoryManager _storyManager;

        public StoryPage()
        {
            InitializeComponent();
            _storyManager = new StoryManager();
            _storyManager.MiniGameRequested += OnMiniGameRequested;
        }

        private void OnMiniGameRequested(MiniGameType gameType)
        {
            var window = Window.GetWindow(this) as StoryWindow;
            if (window == null) return;

            switch (gameType)
            {
                //case MiniGameType.Snowdrops:
                    //window.NavigateTo(new SnowdropsPage());
                    //break;
                //case MiniGameType.MonthsMemory:
                    //window.NavigateTo(new MonthsMemoryPage());
                    //break;
            }
        }
    }
}