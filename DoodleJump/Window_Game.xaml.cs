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

                Camera.Death(Canvas_GameMap, _Player, Label_GameOver);

                return;

            }

            _Player.ChangePlayerPosition();

            Camera.TransformCamera(Canvas_GameMap, _Player);

            PlatformGenerator.GenerateNewPlatform(Canvas_GameMap, _Player);
            
        }

        //Метод вызывается после полной загрузки окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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

    }

}
