using DoodleJump.Scripts;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DoodleJump
{

    public partial class Window_Game : Window
    {

        //Ширина окна с игрой равна 30% от ширины разрешения экрана
        private double _Width = SystemParameters.PrimaryScreenWidth * 0.3 /2;

        //Высота окна с игрой равна 70% от высоты разрешения экрана
        private double _Height = SystemParameters.PrimaryScreenHeight * 0.7 /2;

        //Применить настройки к окну
        private void _AcceptWindowSettings()
        {

            //Устанавливаем ширину текущего окна
            this.Width = _Width;

            //Устанавливаем высоту текущего окна
            this.Height = _Height;

            //Устанавливаем отступ слева для текущего окна
            this.Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);

            //Устанавливаем отступ сверху для текущего окна
            this.Left = (SystemParameters.PrimaryScreenWidth / 2) - (this.Width / 2);

        }

        //Инициализируем окно
        public Window_Game() { InitializeComponent(); }

        //Таймер
        private DispatcherTimer _Timer;

        //Игрок
        private Player _Player; 

        //Каждый шаг таймера
        private void _TimerTick(object sender, EventArgs e)
        {

            //Если игрок не создан, закрываем метод
            if(_Player == null ) { return; }

            //Если игрок мертв
            if(!_Player.isAlive)
            {

                //Останавливаем таймер
                _Timer.Stop();

                //Создаем окно game over
                Window_Gameover window_Gameover = new Window_Gameover(_Player.score);

                //Вызываем окно game over
                window_Gameover.Show();

                //Закрываем текущее окно
                this.Close();

                return;

            }

            //Задаем движение игрока (влево или вправо)
            _MovePlayer();

            //Меняем координаты/позицию игрока на игровом поле
            _Player.ChangePlayerPosition();

            //Обновляем количество очков в шапке
            _UpdateScore();

            //Перемещаем "камеру"
            Camera.TransformCamera(Canvas_GameMap, _Player);

            //Генерируем новые платформы
            PlatformGenerator.GenerateNewPlatform(Canvas_GameMap, _Player);
            
        }

        //Обновление очков в шапке окна
        private void _UpdateScore()
        {

            //Обновляем шапку
            this.Title = "DoodleJump | Score: " + _Player.score;

        }

        //Метод запуска игры и первоначальной настройки
        private void _StartGame()
        {

            //Настраиваем таймер на 1 мс
            _Timer = new DispatcherTimer();

            _Timer.Tick += new EventHandler(_TimerTick);

            //Устанавливаем интервал в 1 мс
            _Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            //Запускаем таймер
            _Timer.Start();

            //Высчитываем центр по иксу
            double playerX = (Canvas_GameMap.ActualWidth / 2);

            //Высчитываем нижнюю координату (~ 0.05 высоты окна от низа)
            double playerY = (Canvas_GameMap.ActualHeight * 0.95);

            //Создаем игрока: канвас, скорость, прыжок, локация
            _Player = new Player(Canvas_GameMap, _Width * 0.015, _Height * 0.02, new Location(playerX, playerY));

            //Задаем ширину игрока
            _Player.Width = Canvas_GameMap.ActualWidth * 0.1;

            //Задаем высоту игрока
            _Player.Height = Canvas_GameMap.ActualHeight * 0.07;

            //Генерируем новые платформы при необходимости
            PlatformGenerator.GenerateNewPlatform(Canvas_GameMap, _Player);

        }

        //Метод вызывается после полной загрузки окна
        private void _Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Применяем настройки окна по загрузке
            _AcceptWindowSettings();

            //Запускаем игру
            _StartGame();

        }

        //Движение игрока
        private void _MovePlayer()
        {

            //Если игрок двигается влево
            if (_LeftPress)
            {

                //НЕ сбрасываем скорость
                _Player.resetVelocity = false;

                //Двигаем игрока влево
                _Player.MoveLeft();

            }
            //Если игрок двигается вправо
            else if (_RightPress)
            {

                //НЕ сбрасываем скорость
                _Player.resetVelocity = false;

                //Двигаем игрока вправо
                _Player.MoveRight();

            }

        }

        //Если нажата левая клавиша
        private bool _LeftPress = false;

        //Если нажата правая клавиша
        private bool _RightPress = false;

        //Если нажата какая-либо клавиша, то вызывается этот метод
        private void _Window_KeyDown(object sender, KeyEventArgs e)
        {

            //Если нажата клавиша A, то начинаем движение влево
            if(e.Key == Key.A)
            {

                //Говорим, что игрок двигается влево
                _LeftPress = true;

            }

            //Если нажата клавиша D, то начинаем движение вправо
            else if (e.Key == Key.D)
            {

                //Говорим, что игрок двигается вправо
                _RightPress = true;

            }

        }

        //Если какая-либо клавиша отпущена, то вызывается этот метод
        private void _Window_KeyUp(object sender, KeyEventArgs e)
        {

            //Если отпущена клавиша A, то прекращаем движение влево
            if (e.Key == Key.A)
            {

                //Говорим, что движение влево прекращаем 
                _LeftPress = false;

            }

            //Если отпущена клавиша D, то прекращаем движение вправо
            else if (e.Key == Key.D)
            {

                //Говорим, что движение вправо прекращаем 
                _RightPress = false;

            }

            //Говорим, что нужно сбросить скорость до нуля (постепенно)
            _Player.resetVelocity = true;

        }

    }

}
