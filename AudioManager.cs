using System.Windows.Media;

namespace MarshakGame
{
    public static class AudioManager
    {
        private static readonly MediaPlayer _player = new MediaPlayer();

        public static double Volume
        {
            get => _player.Volume;
            set => _player.Volume = value;
        }

        public static void PlaySound(string filePath)
        {
            _player.Open(new System.Uri(filePath, System.UriKind.Relative));
            _player.Play();
        }
    }
}