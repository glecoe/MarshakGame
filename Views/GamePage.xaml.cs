using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MarshakGame.Views
{
    public partial class GamePage : Page
    {
        private class Scene
        {
            public string ImagePath { get; set; }
            public string Text { get; set; }
        }

        private List<Scene> scenes;
        private int currentSceneIndex = 0;

        public GamePage()
        {
            InitializeComponent();
            LoadScenes();
            ShowScene(0);
        }

        private void LoadScenes()
        {
            scenes = new List<Scene>
            {
                new Scene
                {
                    ImagePath = "Assets/Images/scene1.png",
                    Text = "Солнечным зимним днем заяц предложил белкам играть в горелки – солнце окликать, весну зазывать."
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene2.png",
                    Text = "За их играми стала наблюдать Падчерица, которую злая Мачеха послала в лес за хворостом и дровами."
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene3.png",
                    Text = "Вскоре на поляну вышел Солдат с санями. Падчерица рассказала ему о проказах белок и зайца, но тот совсем не удивился: Под Новый год и не такое случается! Он рассказал, как однажды его деду довелось со всеми двенадцатью месяцами встретиться под Новый год. "
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene4.png",
                    Text = "Солдат поведал, что должен привезти елку для самой королевы, которая была ровесницей Падчерицы и после смерти родителей осталась круглой сиротой."
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene5.png",
                    Text = "В роскошной классной комнате Профессор проводил урок. Королеве всего четырнадцать лет, но она ужасно избалованна и капризна. Урок по чистописанию был прерван Канцлером, которому потребовалось срочно подписать бумаги."
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene6.png",
                    Text = "Нужно было выбрать – казнить или помиловать человека, и Королева написала ”казнить” – потому что так короче. Мудрый Профессор принялся укорять девочку тем, что она решила судьбу человека, даже не задумавшись."
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene7.png",
                    Text = "Затем капризной Королеве взбрело в голову, чтобы наступил апрель, и на новогоднем банкете были подснежники. Она издала указ, в котором объявила о начале весны и пообещала щедро наградить того, кто принесет во дворец подснежники."
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene8.png",
                    Text = "В маленьком домике на окраине города Мачеха с Дочкой обсуждали приказ Королевы. Им очень хотелось получить обещанную награду, но где зимой отыщешь подснежники?"
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene9.png",
                    Text = "Они решили отправить в лес Падчерицу, чтобы та принесла им весенние цветы. Падчерица принялась умолять Мачеху пожалеть ее: на улице темно, воет метель, да какие же теперь подснежники – ведь зима... Но только жадная старуха и слышать ничего не хотела – дав корзинку побольше, она захлопнула за Падчерицей дверь."
                },
                new Scene
                {
                    ImagePath = "Assets/Images/mau.png",
                    Text = "Замерзшей девочке было очень страшно в темном лесу. Вдруг вдалеке ей почудился золотой огонек, и дымком теплым как будто запахло."
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene11.png",
                    Text = "Она обрадовалась и пошла навстречу огоньку, который оказался большим пылающим костром. Вокруг него сидели и грелись все двенадцать братьев-месяцев: трое старых, трое пожилых, трое молодых, а последние трое – совсем еще юноши."
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene12.png",
                    Text = "Набравшись смелости, девочка подошла к ним и рассказала, что злая Мачеха заставила ее пойти в лес и собрать подснежники."
                },
                //МИНИ-ИГРА ПЕРВАЯ

                new Scene
                {
                    ImagePath = "Assets/Images/scene13.png",
                    Text = "Чтобы помочь ей, братья решили на часок уступить место Апрелю."
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene14.png",
                    Text = "В лесу и на поляне все изменилось: растаял снег, появилась зеленая травка, распустились подснежники."
                },
                //ВТОРАЯ МИНИ-ИГРА

                new Scene
                {
                    ImagePath = "Assets/Images/scene15.png",
                    Text = "Девочка принялась собирать цветы, и вскоре наполнили ими большую корзину."
                },
                new Scene
                {
                    ImagePath = "Assets/Images/scene16.png",
                    Text = "Она очень понравилась юному Апрелю, который подарил ей свое колечко. Если приключится беда, нужно бросить колечко, произнести волшебные слова и все двенадцать месяцев придут на помощь. "
                },

            };
        }

        private void ShowScene(int index)
        {
            if (index < 0 || index >= scenes.Count)
                return;

            var scene = scenes[index];

            try
            {
                string fullPath = System.IO.Path.GetFullPath(scene.ImagePath);
                BackgroundImage.Source = new BitmapImage(new Uri(fullPath));
                DialogueText.Text = scene.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке сцены:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentSceneIndex++;
            if (currentSceneIndex >= scenes.Count)
            {
                // Конец новеллы – вернуться в меню или что-то ещё
                if (Window.GetWindow(this) is MainWindow main)
                    main.NavigateTo(new MenuPage());
                return;
            }

            ShowScene(currentSceneIndex);
        }
    }
}
