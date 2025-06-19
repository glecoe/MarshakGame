using System.Windows;

namespace MarshakGame
{
    public static class GameSettings
    {
        public static double Volume { get; set; } = 0.5;
        public static int ResolutionIndex { get; set; } = 1; // По умолчанию 1280x720
        public static Size Resolution { get; set; } = new Size(1280, 720);
        public static bool IsFullscreen { get; set; } = false;
    }
}