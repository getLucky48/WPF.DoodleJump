using DoodleJump.Scripts;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DoodleJump
{

    public partial class Window_Game : Window
    {

        //Инициализируем окно
        public Window_Game() { InitializeComponent(); }

        //Таймер
        private DispatcherTimer _Timer;

        //Игрок
        private Player _Player; 

        //Каждый шаг таймера
        private void TimerTick(object sender, EventArgs e)
        {

            if (!_Player.isAlive)
            {

                Camera.Death(Canvas_GameMap, _Player, Grid_GameOver);

                return;

            }

            _Player.ChangePlayerPosition();

            UpdateScore();

            Camera.TransformCamera(Canvas_GameMap, _Player);

            PlatformGenerator.GenerateNewPlatform(Canvas_GameMap, _Player);
            
        }

        //Обновление очков в шапке окна
        private void UpdateScore()
        {

            this.Title = "DoodleJump | Score: " + _Player.score;

        }

        //Метод запуска игры и первоначальной настройки
        private void StartGame()
        {

            //Скрываем надпись о проигрыше
            Grid_GameOver.Visibility = Visibility.Hidden;

            //Настраиваем таймер на 1 мс
            _Timer = new DispatcherTimer();

            _Timer.Tick += new EventHandler(TimerTick);

            _Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            _Timer.Start();

            //Создаем игрока: канвас, скорость, прыжок, локация
            _Player = new Player(Canvas_GameMap, 10.0, 15.0, new Location(250.0, 650.0));

            //Генерируем новые платформы при необходимости
            PlatformGenerator.GenerateNewPlatform(Canvas_GameMap, _Player);

        }

        //Метод вызывается после полной загрузки окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Запускаем игру
            StartGame();

        }
               
        private void Window_KeyDown(object sender, KeyEventArgs e)
        { 

            if (e.Key == Key.A)
            {

                _Player.resetVelocity = false;

                _Player.MoveLeft();

            }

            if (e.Key == Key.D)
            {

                _Player.resetVelocity = false;

                _Player.MoveRight();

            }

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

            _Player.resetVelocity = true;

        }

        private void Button_PlayAgain_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //Останавливаем таймер, иначе отсчет времени в игре сломается
            _Timer.Stop();

            //Запускаем игру снова
            StartGame();

        }

        private void Button_Start_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            MainWindow mainWindow = new MainWindow();

            mainWindow.Show();

            this.Close();

        }

    }

}
