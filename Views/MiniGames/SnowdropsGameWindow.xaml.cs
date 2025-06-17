using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MarshakGame.Views.MiniGames
{
    public partial class SnowdropsGameWindow : Window
    {
        private readonly Random random = new Random();
        private int score = 0;
        private double fireSize = 100;
        private bool isGameRunning = false;
        private readonly List<GameObject> gameObjects = new List<GameObject>();
        private readonly DispatcherTimer gameTimer = new DispatcherTimer();
        private readonly DispatcherTimer spawnTimer = new DispatcherTimer();
        private readonly DispatcherTimer countdownTimer = new DispatcherTimer();
        private int timeLeft = 30;
        private readonly int returnSceneIndex;

        public SnowdropsGameWindow(int sceneIndex)
        {
            returnSceneIndex = sceneIndex + 1;
            InitializeComponent();
            InitializeGame();
            Loaded += OnWindowLoaded;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            CenterFireImage();
        }

        private void InitializeGame()
        {
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += GameLoop;

            spawnTimer.Interval = TimeSpan.FromSeconds(1);
            spawnTimer.Tick += SpawnObject;

            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTick;
        }

        private void CenterFireImage()
        {
            if (GameCanvas.ActualWidth > 0)
            {
                Canvas.SetLeft(FireImage, (GameCanvas.ActualWidth - FireImage.Width) / 2);
            }
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            gameObjects.Clear();
            GameCanvas.Children.Clear();

            // Добавляем основные элементы обратно
            GameCanvas.Children.Add(BackgroundImage);
            GameCanvas.Children.Add(FireImage);
            GameCanvas.Children.Add(ScoreText);
            GameCanvas.Children.Add(TimerText);

            score = 0;
            timeLeft = 30;
            fireSize = 250;
            UpdateFireSize();

            ScoreText.Text = $"Очки: {score}";
            TimerText.Text = $"Время: {timeLeft}с";

            StartScreen.Visibility = Visibility.Collapsed;
            GameOverScreen.Visibility = Visibility.Collapsed;

            isGameRunning = true;
            gameTimer.Start();
            spawnTimer.Start();
            countdownTimer.Start();
        }

        private void UpdateFireSize()
        {
            FireImage.Width = fireSize;
            FireImage.Height = fireSize * 1.5;
            CenterFireImage();
        }

        private void SpawnObject(object sender, EventArgs e)
        {
            if (!isGameRunning) return;

            bool isLog = random.Next(0, 2) == 0;
            string imagePath = isLog ? "brevno.png" : "snowball.png";

            try
            {
                var image = new Image
                {
                    Width = isLog ? 150 : 90,
                    Height = isLog ? 90 : 90,
                    Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/Images/{imagePath}")),
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания объекта: {ex.Message}");
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
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

        private void CountdownTick(object sender, EventArgs e)
        {
            timeLeft--;
            TimerText.Text = $"Время: {timeLeft}с";

            if (timeLeft <= 0)
            {
                EndGame(true);
            }
        }

        private void GameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isGameRunning) return;

            Point clickPoint = e.GetPosition(GameCanvas);

            foreach (var obj in gameObjects.ToList())
            {
                Rect bounds = new Rect(obj.X, obj.Y, obj.Shape.Width, obj.Shape.Height);

                if (bounds.Contains(clickPoint))
                {
                    ProcessObjectClick(obj);
                    break;
                }
            }
        }

        private void ProcessObjectClick(GameObject obj)
        {
            obj.WasClicked = true;
            score += Math.Max(0, obj.Points);
            fireSize = Math.Min(350, Math.Max(0, fireSize + (obj.Points > 0 ? 50 : -20)));

            ScoreText.Text = $"Очки: {score}";
            UpdateFireSize();

            AnimateObjectRemoval(obj);

            if (fireSize <= 0)
            {
                EndGame(false);
            }
        }

        private void AnimateObjectRemoval(GameObject obj)
        {
            var animation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200)
            };

            animation.Completed += (s, _) => RemoveObject(obj);
            obj.Shape.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        private void RemoveObject(GameObject obj)
        {
            GameCanvas.Children.Remove(obj.Shape);
            gameObjects.Remove(obj);
        }

        private void EndGame(bool isWin)
        {
            isGameRunning = false;
            gameTimer.Stop();
            spawnTimer.Stop();
            countdownTimer.Stop();

            GameResultText.Text = isWin ? "Победа!" : "Поражение!";
            NextButton.Visibility = isWin ? Visibility.Visible : Visibility.Collapsed;
            GameOverScreen.Visibility = Visibility.Visible;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateTo(new GamePage(returnSceneIndex));
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            gameTimer.Stop();
            spawnTimer.Stop();
            countdownTimer.Stop();
        }
    }

    class GameObject
    {
        public Image Shape { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Speed { get; set; }
        public int Points { get; set; }
        public bool WasClicked { get; set; }
    }
}