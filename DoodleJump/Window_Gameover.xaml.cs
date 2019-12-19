using System.Windows;
using System.Windows.Input;

namespace DoodleJump
{

    public partial class Window_Gameover : Window
    {

        //Количество очков
        private long _Score;

        //Конструктор класса
        public Window_Gameover()
        {

            //Устанавливаем начальные очки
            _Score = 0;

            //Инициализируем компонент
            InitializeComponent();

        }

        //Конструктор класса (с аргументом)
        public Window_Gameover(long tScore)
        {

            //Устанавливаем заданное число очков
            _Score = tScore;

            //Инициализируем компонент
            InitializeComponent();

        }

        //Если был произведен клик по картинке PLAYAGAIN, то вызывается этот метод
        private void Button_PlayAgain_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //Создать окно с игрой
            Window_Game gameWindow = new Window_Game();

            //Открыть окно с игрой
            gameWindow.Show();

            //Закрыть текущее окно
            this.Close();

        }

        //Если был произведен клик по картинке MENU, то вызывается этот метод
        private void Button_Menu_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //Создать лаунчер
            MainWindow mainWindow = new MainWindow();

            //Открыть лаунчер
            mainWindow.Show();

            //закрыть текущее окно
            this.Close();

        }

        //Когда окно загружено, вызывается этот метод
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            //Устанавливаем количество очков в элемент label
            Label_Score.Content = "Score: " + _Score.ToString();

        }

    }

}
