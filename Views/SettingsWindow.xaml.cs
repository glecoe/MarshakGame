using System.Windows;
using MarshakGame.Properties;

namespace MarshakGame.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            VolumeSlider.Value = Settings.Default.Volume;
            FullscreenCheckBox.IsChecked = Settings.Default.Fullscreen;
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Volume = (int)VolumeSlider.Value;
            Settings.Default.Fullscreen = FullscreenCheckBox.IsChecked ?? false;
            Settings.Default.Save();
            Close();
        }
    }
}