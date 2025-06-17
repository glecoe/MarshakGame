using System.Windows.Controls;

namespace MarshakGame.Views.MiniGames
{
    public abstract class MiniGameBasePage : Page
    {
        public abstract bool IsCompleted { get; }
    }
}