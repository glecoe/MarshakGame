using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MarshakGame.Views
{
    public partial class TwelveMonthsQuizPage : Page
    {
        private int currentQuestionIndex = 0;

        private List<Question> questions = new List<Question>
        {
            new Question(
                "С кем Падчерица разговаривала в лесу?",
                "солдат",
                "Это человек в военной форме.",
                new List<string> { "Король", "Солдат", "Месяц", "Лесник" }
            ),
            new Question(
                "Чего хотела капризная Королева?",
                "подснежники",
                "Эти цветы обычно появляются весной.",
                new List<string> { "Подснежники", "Ромашки", "Розы", "Ландыши" }
            ),
            new Question(
                "Кто отправил Падчерицу в лес?",
                "мачеха",
                "Это злая родственница.",
                new List<string> { "Отец", "Сестра", "Королева", "Мачеха" }
            ),
            new Question(
                "Вокруг чего сидели братья-месяцы в лесу?",
                "костер",
                "Он даёт тепло и свет.",
                new List<string> { "Стол", "Дерево", "Костер", "Ручей" }
            ),
            new Question(
                "Какой месяц помог девочке?",
                "апрель",
                "Это весенний месяц.",
                new List<string> { "Январь", "Декабрь", "Июнь", "Апрель" }
            )
        };
        public TwelveMonthsQuizPage()
        {
            InitializeComponent();
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartScreen.Visibility = Visibility.Collapsed;
            StartQuiz();
        }

        private void StartQuiz()
        {
            currentQuestionIndex = 0;
            FeedbackText.Text = "";
            HintText.Text = "";
            GameOverScreen.Visibility = Visibility.Collapsed;
            QuestionPanel.Visibility = Visibility.Visible;
            ShowCurrentQuestion();
        }

        private void ShowCurrentQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                var question = questions[currentQuestionIndex];
                QuestionText.Text = question.Text;
                FeedbackText.Text = "";
                HintText.Text = "";

                AnswerButtonsPanel.Children.Clear();

                foreach (var option in question.Options)
                {
                    var button = new Button
                    {
                        Content = option,
                        FontSize = 24,
                        Padding = new Thickness(20, 10, 20, 10),
                        Margin = new Thickness(0, 5, 0, 5),
                        Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(74, 46, 29)),
                        Foreground = System.Windows.Media.Brushes.White,
                        BorderBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 153, 0))
                    };

                    button.Click += (sender, e) =>
                    {
                        CheckAnswer(option);
                    };

                    AnswerButtonsPanel.Children.Add(button);
                }
            }
            else
            {
                EndQuiz();
            }
        }

        private void CheckAnswer(string selectedAnswer)
        {
            string correctAnswer = questions[currentQuestionIndex].CorrectAnswer.ToLower();

            if (selectedAnswer.ToLower() == correctAnswer)
            {
                FeedbackText.Text = "Молодец! Это правильный ответ.";
                HintText.Text = "";
                currentQuestionIndex++;

                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1.5);
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    ShowCurrentQuestion();
                };
                timer.Start();
            }
            else
            {
                FeedbackText.Text = "Неправильный ответ. Попробуй ещё раз.";
                HintText.Text = $"Подсказка: {questions[currentQuestionIndex].Hint}";
            }
        }

        private void EndQuiz()
        {
            QuestionPanel.Visibility = Visibility.Collapsed;
            GameOverScreen.Visibility = Visibility.Visible;
            GameResultText.Text = "Поздравляем!";
            FinalScoreText.Text = $"Вы ответили на все вопросы!";
        }

        private void RestartGame_Click(object sender, RoutedEventArgs e)
        {
            StartQuiz();
        }
        private void EndGame_Click(object sender, RoutedEventArgs e)
        {
            // Сброс счетчика текущей сцены
            GamePage.SavedSceneIndex = 0;

            // Переход на главное меню
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateTo(new MenuPage());
            }
        }
    }

    public class Question
    {
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }
        public string Hint { get; set; }
        public List<string> Options { get; set; }

        public Question(string text, string correctAnswer, string hint, List<string> options)
        {
            Text = text;
            CorrectAnswer = correctAnswer;
            Hint = hint;
            Options = options;
        }
    }
}

