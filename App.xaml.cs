using System.Windows;
using MarshakGame.Properties;

namespace MarshakGame
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Установка громкости при запуске
            AudioManager.Volume = Settings.Default.Volume / 100.0;

            // Подписка на изменение настроек
            Settings.Default.PropertyChanged += (s, args) =>
            {
                if (args.PropertyName == "Volume")
                {
                    AudioManager.Volume = Settings.Default.Volume / 100.0;
                }
            };
        }
    }
}