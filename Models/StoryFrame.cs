public class StoryFrame
{
    public string Text { get; set; }
    public string ImagePath { get; set; } // Опционально
    public Action MiniGameAction { get; set; } // Если нужно запустить мини-игру
}