using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MarshakGame.Views
{
    public partial class FireGamePage : Page
    {
        private Random random = new Random();
        private int score = 0;
        private double fireSize = 100;
        private const double MaxFireSize = 350;
        private const double MinFireSize = 0;
        private bool isGameRunning = false;
        private List<GameObject> gameObjects = new List<GameObject>();
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private DispatcherTimer spawnTimer = new DispatcherTimer();
        private DispatcherTimer countdownTimer = new DispatcherTimer();
        private DispatcherTimer fireDecreaseTimer = new DispatcherTimer();
        private int timeLeft = 30;

        public FireGamePage()
        {
            InitializeComponent();
            SetupTimers();
        }

        private void SetupTimers()
        {
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += GameLoop;

            spawnTimer.Interval = TimeSpan.FromSeconds(1);
            spawnTimer.Tick += SpawnObject;

            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTick;

            fireDecreaseTimer.Interval = TimeSpan.FromSeconds(1);
            fireDecreaseTimer.Tick += DecreaseFire;
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void RestartGame_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void ContinueStory_Click(object sender, RoutedEventArgs e)
        {
            // Устанавливаем сохраненный индекс на 12 (чтобы перейти к 13-й сцене)
            GamePage.SavedSceneIndex = 12;

            if (NavigationService != null)
            {
                // Создаем новый экземпляр с сохраненным прогрессом
                NavigationService.Navigate(new GamePage());
            }
        }

        private void StartGame()
        {
            foreach (var obj in gameObjects.ToList())
            {
                GameCanvas.Children.Remove(obj.Shape);
                gameObjects.Remove(obj);
            }

            score = 0;
            fireSize = 250;
            timeLeft = 30;

            FireImage.Width = fireSize;
            FireImage.Height = fireSize * 1.5;
            UpdateFireSize();

            ScoreText.Text = $"Очки: {score}";
            TimerText.Text = $"Время: {timeLeft}с";

            StartScreen.Visibility = Visibility.Collapsed;
            GameOverScreen.Visibility = Visibility.Collapsed;

            isGameRunning = true;
            gameTimer.Start();
            spawnTimer.Start();
            countdownTimer.Start();
            fireDecreaseTimer.Start();
        }

        private void UpdateFireSize()
        {
            FireImage.Width = fireSize;
            FireImage.Height = fireSize * 1.5;

            Canvas.SetLeft(FireImage, 960 - fireSize / 2);
            Canvas.SetTop(FireImage, 1080 - 250 - FireImage.Height);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (!isGameRunning) return;

            foreach (var obj in gameObjects.ToList())
            {
                obj.Y += obj.Speed;
                Canvas.SetTop(obj.Shape, obj.Y);

                if (obj.Y > GameCanvas.ActualHeight)
                {
                    GameCanvas.Children.Remove(obj.Shape);
                    gameObjects.Remove(obj);
                }
            }
        }

        private void SpawnObject(object sender, EventArgs e)
        {
            if (!isGameRunning) return;

            bool isLog = random.Next(0, 2) == 0;

            Image image = new Image
            {
                Width = isLog ? 150 : 90,
                Height = isLog ? 90 : 90,
                Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/Images/{(isLog ? "brevno.png" : "snowball.png")}")),
                IsHitTestVisible = true
            };

            double x = random.Next(0, (int)(GameCanvas.ActualWidth - image.Width));
            double y = -image.Height;

            Canvas.SetLeft(image, x);
            Canvas.SetTop(image, y);

            GameCanvas.Children.Add(image);

            gameObjects.Add(new GameObject
            {
                Shape = image,
                X = x,
                Y = y,
                Speed = random.Next(3, 7),
                Points = isLog ? 10 : -15,
                WasClicked = false
            });
        }

        private void CountdownTick(object sender, EventArgs e)
        {
            if (!isGameRunning) return;

            timeLeft--;
            TimerText.Text = $"Время: {timeLeft}с";

            if (timeLeft <= 0)
            {
                EndGame(true);
            }
        }

        private void DecreaseFire(object sender, EventArgs e)
        {
            if (!isGameRunning) return;

            fireSize = Math.Max(MinFireSize, fireSize - 25);
            UpdateFireSize();

            if (fireSize <= MinFireSize)
            {
                EndGame(false);
            }
        }

        private void GameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isGameRunning) return;

            Point clickPoint = e.GetPosition(GameCanvas);

            foreach (var obj in gameObjects.ToList().Where(obj => !obj.WasClicked))
            {
                Rect bounds = new Rect(obj.X, obj.Y, obj.Shape.Width, obj.Shape.Height);

                if (bounds.Contains(clickPoint))
                {
                    obj.WasClicked = true;

                    fireSize = Math.Min(MaxFireSize, Math.Max(MinFireSize, fireSize + (obj.Points > 0 ? 50 : -20)));
                    UpdateFireSize();

                    score += Math.Max(0, obj.Points);
                    ScoreText.Text = $"Очки: {score}";

                    DoubleAnimation fadeOut = new DoubleAnimation
                    {
                        From = 1,
                        To = 0,
                        Duration = TimeSpan.FromMilliseconds(200)
                    };

                    fadeOut.Completed += (s, args) =>
                    {
                        GameCanvas.Children.Remove(obj.Shape);
                        gameObjects.Remove(obj);
                    };

                    obj.Shape.BeginAnimation(Image.OpacityProperty, fadeOut);

                    if (fireSize <= MinFireSize)
                    {
                        EndGame(false);
                    }

                    break;
                }
            }
        }

        private void EndGame(bool isWin)
        {
            isGameRunning = false;
            gameTimer.Stop();
            spawnTimer.Stop();
            countdownTimer.Stop();
            fireDecreaseTimer.Stop();

            GameResultText.Text = isWin ? "Победа!" : "Поражение!";
            FinalScoreText.Text = $"Ваш результат: {score} очков";

            NextButton.Visibility = isWin ? Visibility.Visible : Visibility.Collapsed;
            GameOverScreen.Visibility = Visibility.Visible;
        }
    }

    public class GameObject
    {
        public Image Shape { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Speed { get; set; }
        public int Points { get; set; }
        public bool WasClicked { get; set; }
    }
}