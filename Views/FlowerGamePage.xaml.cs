using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MarshakGame.Views
{
    public partial class FlowerGamePage : Page
    {
        private Random _random = new Random();
        private int _score = 0;
        private int _targetCount = 0;
        private int _lives = 3;

        private List<string> snowdropImages = new() { "Assets/Images/snowdrop1.png", "Assets/Images/snowdrop2.png", "Assets/Images/snowdrop3.png" };
        private List<string> otherFlowerImages = new() { "Assets/Images/flower1.png", "Assets/Images/flower2.png" };

        private SoundPlayer collectSound = new SoundPlayer("Assets/Images/collect.wav");
        private SoundPlayer wrongSound = new SoundPlayer("Assets/Images/wrong.wav");

        public FlowerGamePage()
        {
            InitializeComponent();
            StartScreen.Visibility = Visibility.Visible; // Показываем стартовый экран
        }

        private void SafePlay(SoundPlayer player)
        {
            try { player.Play(); } catch { }
        }

        // Новый метод: переход на экран с подсказкой
        private void StartHintScreen_Click(object sender, RoutedEventArgs e)
        {
            StartScreen.Visibility = Visibility.Collapsed;
            HintScreen.Visibility = Visibility.Visible; // Показываем подсказку
        }

        // Начало игры после подсказки
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            HintScreen.Visibility = Visibility.Collapsed;
            InitGame();
        }

        private void InitGame()
        {
            _score = 0;
            _lives = 3;
            _targetCount = _random.Next(5, 11);
            UpdateScoreText();
            FlowerCanvas.Children.Clear();
            HeartsPanel.Children.Clear();

            for (int i = 0; i < 3; i++)
                HeartsPanel.Children.Add(new Image
                {
                    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/heart.png")),
                    Width = 50,
                    Height = 50
                });

            SpawnFlowers();
            UpdateBasket();
        }

        private void UpdateScoreText()
        {
            ScoreText.Text = $"Собрано: {_score}/{_targetCount}";
        }

        private void SpawnFlowers()
        {
            List<Rect> occupied = new();

            void PlaceFlower(string path, bool isTarget)
            {
                int x, y;
                Rect newRect;
                do
                {
                    x = _random.Next(100, 1450);
                    y = _random.Next(600, 900);
                    newRect = new Rect(x, y, 100, 100);
                } while (occupied.Any(r => r.IntersectsWith(newRect)) || (x < 300 && y > 800));

                occupied.Add(newRect);
                var img = new Image
                {
                    Source = new BitmapImage(new Uri($"pack://application:,,,/{path}")),
                    Width = 80,
                    Height = 80,
                    Tag = isTarget ? "Snowdrop" : "Other",
                    RenderTransformOrigin = new Point(0.5, 0.5)
                };

                if (_random.NextDouble() < 0.5)
                    img.RenderTransform = new ScaleTransform { ScaleX = -1, ScaleY = 1 };

                Canvas.SetLeft(img, x);
                Canvas.SetTop(img, y);
                img.MouseLeftButtonDown += Flower_Click;

                FlowerCanvas.Children.Add(img);
            }

            for (int i = 0; i < _targetCount; i++)
                PlaceFlower(snowdropImages[_random.Next(snowdropImages.Count)], true);

            int otherCount = _random.Next(5, 16);
            for (int i = 0; i < otherCount; i++)
                PlaceFlower(otherFlowerImages[_random.Next(otherFlowerImages.Count)], false);
        }

        private void Flower_Click(object sender, MouseButtonEventArgs e)
        {
            var img = sender as Image;
            if (img == null) return;

            FlowerCanvas.Children.Remove(img);

            if ((string)img.Tag == "Snowdrop")
            {
                SafePlay(collectSound);
                _score++;
                UpdateScoreText();
                UpdateBasket();

                if (_score >= _targetCount)
                    EndGame(true);
            }
            else
            {
                SafePlay(wrongSound);
                _lives--;
                if (HeartsPanel.Children.Count > 0)
                    HeartsPanel.Children.RemoveAt(0);

                if (_lives <= 0)
                    EndGame(false);
            }
        }

        private void EndGame(bool victory)
        {
            GameOverScreen.Visibility = Visibility.Visible;
            
            if (victory)
            {
                GameResultText.Text = "Победа!";
                NextButton.Visibility = Visibility.Visible; // Показываем кнопку "Дальше"
            }
            else
            {
                GameResultText.Text = "Попробуй ещё раз!";
                NextButton.Visibility = Visibility.Collapsed;
            }
            
            FinalScoreText.Text = $"Собрано подснежников: {_score}/{_targetCount}";
        }

        // Рестарт игры
        private void RestartGame_Click(object sender, RoutedEventArgs e)
        {
            GameOverScreen.Visibility = Visibility.Collapsed;
            InitGame();
        }

        // Продолжение истории (переход на другую страницу)
        private void ContinueStory_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null)
            {
                NavigationService.Navigate(new GamePage { CurrentSceneIndex = 14 });
            }
        }

        private void UpdateBasket()
        {
            string imagePath;
            if (_score >= 6)
                imagePath = "Assets/Images/basket_full.png";
            else if (_score >= 2)
                imagePath = "Assets/Images/basket_half.png";
            else
                imagePath = "Assets/Images/basket_empty.png";

            BasketImage.Source = new BitmapImage(new Uri($"pack://application:,,,/{imagePath}"));
        }
    }
}