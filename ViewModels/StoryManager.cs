using MarshakGame.Models;
using MarshakGame.Commands;
using System.Windows.Input;

namespace MarshakGame.ViewModels
{
    public class StoryManager
    {
        public event Action<MiniGameType> MiniGameRequested;
        public ICommand NextCommand { get; }

        public StoryManager()
        {
            NextCommand = new RelayCommand(_ => NextFrame());
        }

        private void NextFrame()
        {
            // Логика перехода между кадрами
        }
    }
}