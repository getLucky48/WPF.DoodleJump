using DoodleJump.Scripts;
using System.Windows;
using System.Windows.Input;

namespace DoodleJump
{

    public partial class MainWindow : Window
    {

        //Конструкор класса
        public MainWindow()
        {

            //Инициализируем компонент
            InitializeComponent();

        }

        //Когда окно загружено, вызывается этот метод
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Создаем игрока (для вида в меню)
            Player player = new Player(Canvas_Launcher, 0.1, 0.1, new Location(35.0, 200.0));

        }

        //Если был произведен клик по картинке START, то вызывается этот метод
        private void Button_Start_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //Создаем окно с игрой
            Window_Game windowGame = new Window_Game();

            //Открываем окно с игрой
            windowGame.Show();

            //Закрываем текущее окно
            this.Close();

        }

        //Если был произведен клик по картинке EXIT, то вызывается этот метод
        private void Button_Exit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //Закрываем текущее окно
            this.Close();

        }

    }

}
