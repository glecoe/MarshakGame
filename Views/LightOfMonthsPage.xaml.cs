using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects; // Добавлено для DropShadowEffect
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace MarshakGame.Views
{
    public partial class LightOfMonthsPage : Page
    {
        private List<Ellipse> fireEllipses = new List<Ellipse>(); // Огоньки
        private List<Line> lines = new List<Line>(); // Линии между огоньками
        private Ellipse firstSelectedFire = null; // Первый выбранный огонек
        private List<(int, int)> orderedConnections; // Упорядоченные соединения
        private int currentConnectionIndex = 0; // Текущий индекс соединения
        private Ellipse highlightedFire = null; // Подсвеченный огонек

        public LightOfMonthsPage()
        {
            InitializeComponent();
            Loaded += LightOfMonthsPage_Loaded;
        }

        private void LightOfMonthsPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Инициализация отложена до нажатия кнопки "Понятно"
        }

        private void InitializeGame()
        {
            // Очистка канваса
            GameCanvas.Children.Clear();
            fireEllipses.Clear();
            lines.Clear();
            firstSelectedFire = null;
            highlightedFire = null;
            currentConnectionIndex = 0;
            StatusText.Text = "Соедините огоньки в порядке!";

            // Проверка размеров Canvas
            if (GameCanvas.ActualWidth <= 100 || GameCanvas.ActualHeight <= 100)
            {
                StatusText.Text = "Ошибка: размер игрового поля слишком мал!";
                return;
            }

            // Установка размеров Canvas
            GameCanvas.Width = MainGrid.ActualWidth;
            GameCanvas.Height = MainGrid.ActualHeight;

            // Создание 10 огоньков в форме ровной узорной звезды
            CreateStarFires();

            // Определение упорядоченных соединений для узорной звезды
            InitializeOrderedConnections();

            // Подсветка первого огонька
            HighlightNextFire();
        }

        private void CreateStarFires()
        {
            double centerX = GameCanvas.ActualWidth / 2;
            double centerY = GameCanvas.ActualHeight / 2;
            double outerRadius = Math.Min(GameCanvas.ActualWidth, GameCanvas.ActualHeight) / 4; // Радиус внешней звезды
            double innerRadius = outerRadius * 0.382; // Радиус внутренней звезды (золотое сечение)

            // Создаем 10 огоньков: 5 внешних и 5 внутренних
            for (int i = 0; i < 5; i++)
            {
                // Внешние вершины звезды
                double outerAngle = 2 * Math.PI * i / 5 - Math.PI / 2;
                double xOuter = centerX + outerRadius * Math.Cos(outerAngle);
                double yOuter = centerY + outerRadius * Math.Sin(outerAngle);

                Ellipse outerFire = CreateFire(xOuter, yOuter);
                outerFire.Tag = new { Index = i, Point = new Point(xOuter, yOuter) };
                fireEllipses.Add(outerFire);
                GameCanvas.Children.Add(outerFire);

                // Внутренние узлы
                double innerAngle = 2 * Math.PI * i / 5 - Math.PI / 2;
                double xInner = centerX + innerRadius * Math.Cos(innerAngle);
                double yInner = centerY + innerRadius * Math.Sin(innerAngle);

                Ellipse innerFire = CreateFire(xInner, yInner);
                innerFire.Tag = new { Index = i + 5, Point = new Point(xInner, yInner) };
                fireEllipses.Add(innerFire);
                GameCanvas.Children.Add(innerFire);
            }
        }

        private Ellipse CreateFire(double x, double y)
        {
            Ellipse fire = new Ellipse
            {
                Width = 40,
                Height = 40,
                Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Assets/Images/star.png")))
            };
            Canvas.SetLeft(fire, x - 15);
            Canvas.SetTop(fire, y - 15);
            fire.MouseLeftButtonDown += Fire_MouseLeftButtonDown;
            return fire;
        }

        private void InitializeOrderedConnections()
        {
            // Упорядоченные соединения для ровной пентаграммы с внутренними линиями
            orderedConnections = new List<(int, int)>
            {
                (0, 2), // Пентаграмма: 0 -> 2
                (2, 4), // 2 -> 4
                (4, 1), // 4 -> 1
                (1, 3), // 1 -> 3
                (3, 0), // 3 -> 0
                (5, 0), // Внутренняя 5 -> внешняя 0
                (6, 1), // Внутренняя 6 -> внешняя 1
                (7, 2), // Внутренняя 7 -> внешняя 2
                (8, 3), // Внутренняя 8 -> внешняя 3
                (9, 4)  // Внутренняя 9 -> внешняя 4
            };
        }

        private void HighlightNextFire()
        {
            // Очистка предыдущего подсвеченного огонька
            if (highlightedFire != null)
            {
                highlightedFire.BeginAnimation(UIElement.OpacityProperty, null);
                highlightedFire.Opacity = 1.0;
                highlightedFire.Effect = null; // Удаляем эффект свечения
                highlightedFire = null;
            }

            if (currentConnectionIndex >= orderedConnections.Count)
            {
                return;
            }

            int nextFireIndex = firstSelectedFire == null
                ? orderedConnections[currentConnectionIndex].Item1
                : orderedConnections[currentConnectionIndex].Item2;

            highlightedFire = fireEllipses.Find(f => ((dynamic)f.Tag).Index == nextFireIndex);

            if (highlightedFire != null)
            {
                // Добавляем эффект свечения
                DropShadowEffect glowEffect = new DropShadowEffect
                {
                    Color = Colors.Yellow,
                    BlurRadius = 10,
                    ShadowDepth = 0,
                    Opacity = 0.8
                };
                highlightedFire.Effect = glowEffect;

                // Анимация прозрачности огонька
                DoubleAnimation opacityAnimation = new DoubleAnimation
                {
                    From = 0.3, // Более низкая начальная прозрачность для контраста
                    To = 1.0,
                    Duration = TimeSpan.FromSeconds(0.4), // Ускоряем для динамики
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                highlightedFire.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);

                // Анимация свечения (BlurRadius эффекта)
                DoubleAnimation glowAnimation = new DoubleAnimation
                {
                    From = 5,
                    To = 15,
                    Duration = TimeSpan.FromSeconds(0.4),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };
                glowEffect.BeginAnimation(DropShadowEffect.BlurRadiusProperty, glowAnimation);
            }
        }

        private void Fire_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse selectedFire = sender as Ellipse;
            if (selectedFire == null) return;

            dynamic selectedTag = selectedFire.Tag;
            int selectedIndex = selectedTag.Index;

            int expectedIndex = firstSelectedFire == null
                ? orderedConnections[currentConnectionIndex].Item1
                : orderedConnections[currentConnectionIndex].Item2;

            if (selectedIndex != expectedIndex)
            {
                StatusText.Text = "Неверный огонек! Выберите подсвеченный.";
                return;
            }

            if (firstSelectedFire == null)
            {
                firstSelectedFire = selectedFire;
                selectedFire.BeginAnimation(UIElement.OpacityProperty, null);
                selectedFire.Opacity = 0.5;
                selectedFire.Effect = null; // Удаляем свечение при выборе
                HighlightNextFire();
            }
            else
            {
                dynamic firstTag = firstSelectedFire.Tag;
                Point p1 = firstTag.Point;
                Point p2 = selectedTag.Point;

                Line line = new Line
                {
                    X1 = p1.X,
                    Y1 = p1.Y,
                    X2 = p2.X,
                    Y2 = p2.Y,
                    Stroke = Brushes.Yellow,
                    StrokeThickness = 3
                };
                lines.Add(line);
                GameCanvas.Children.Add(line);

                currentConnectionIndex++;

                if (currentConnectionIndex == orderedConnections.Count)
                {
                    StatusText.Text = "";
                    GameResultText.Text = "Победа! Узорная звезда создана!";
                    GameOverScreen.Visibility = Visibility.Visible;
                    NextButton.Visibility = Visibility.Visible;
                    if (highlightedFire != null)
                    {
                        highlightedFire.BeginAnimation(UIElement.OpacityProperty, null);
                        highlightedFire.Opacity = 1.0;
                        highlightedFire.Effect = null; // Удаляем свечение
                    }
                }
                else
                {
                    StatusText.Text = "Соедините следующий огонек!";
                }

                if (firstSelectedFire != null)
                {
                    firstSelectedFire.BeginAnimation(UIElement.OpacityProperty, null);
                    firstSelectedFire.Opacity = 1.0;
                    firstSelectedFire.Effect = null; // Удаляем свечение
                }
                firstSelectedFire = null;

                HighlightNextFire();
            }
        }

        private void StartHintScreen_Click(object sender, RoutedEventArgs e)
        {
            StartScreen.Visibility = Visibility.Collapsed;
            HintScreen.Visibility = Visibility.Visible;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            HintScreen.Visibility = Visibility.Collapsed;
            InitializeGame();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
        }

        private void RestartGame_Click(object sender, RoutedEventArgs e)
        {
            GameOverScreen.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Collapsed;
            InitializeGame();
        }

        private void ContinueStory_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null)
            {
                NavigationService.Navigate(new GamePage { CurrentSceneIndex = 10 });
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (GameCanvas.Children.Count > 0 && GameCanvas.Children[0] is Image background)
            {
                background.Width = GameCanvas.ActualWidth;
                background.Height = GameCanvas.ActualHeight;
            }
            GameCanvas.Width = MainGrid.ActualWidth;
            GameCanvas.Height = MainGrid.ActualHeight;
        }
    }
}