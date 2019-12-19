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
        private double _Width = SystemParameters.PrimaryScreenWidth * 0.6;

        //Высота окна с игрой равна 70% от высоты разрешения экрана
        private double _Height = SystemParameters.PrimaryScreenHeight * 0.7;

        //Применить настройки к окну
        private void _AcceptWindowSettings()
        {

            this.Width = _Width;

            this.Height = _Height;

            this.Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);

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

            if (!_Player.isAlive)
            {

                _Timer.Stop();

                Window_Gameover window_Gameover = new Window_Gameover(_Player.score);

                window_Gameover.Show();

                this.Close();

                return;

            }

            _MovePlayer();

            _Player.ChangePlayerPosition();

            _UpdateScore();

            Camera.TransformCamera(Canvas_GameMap, _Player);

            PlatformGenerator.GenerateNewPlatform(Canvas_GameMap, _Player);
            
        }

        //Обновление очков в шапке окна
        private void _UpdateScore()
        {

            this.Title = "DoodleJump | Score: " + _Player.score;

        }

        //Метод запуска игры и первоначальной настройки
        private void _StartGame()
        {

            //Настраиваем таймер на 1 мс
            _Timer = new DispatcherTimer();

            _Timer.Tick += new EventHandler(_TimerTick);

            _Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            _Timer.Start();

            //Высчитываем центр по иксу
            double playerX = (Canvas_GameMap.ActualWidth / 2);
            //Высчитываем нижнюю координату (~ 0.05 высоты окна от низа)
            double playerY = (Canvas_GameMap.ActualHeight * 0.95);

            //Создаем игрока: канвас, скорость, прыжок, локация
            _Player = new Player(Canvas_GameMap, _Width * 0.015, _Height * 0.02, new Location(playerX, playerY));

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

            if (_LeftPress)
            {

                _Player.resetVelocity = false;

                _Player.MoveLeft();

            }
            else if (_RightPress)
            {

                _Player.resetVelocity = false;

                _Player.MoveRight();

            }

        }

        //Если нажата левая клавиша
        private bool _LeftPress = false;

        //Если нажата правая клавиша
        private bool _RightPress = false;

        private void _Window_KeyDown(object sender, KeyEventArgs e)
        {

            //Если нажата клавиша A, то начинаем движение влево
            if(e.Key == Key.A)
            {

                _LeftPress = true;

            }

            //Аналогично
            else if(e.Key == Key.D)
            {

                _RightPress = true;

            }

        }

        private void _Window_KeyUp(object sender, KeyEventArgs e)
        {

            //Если отпущена клавиша A, то прекращаем движение влево
            if (e.Key == Key.A)
            {

                _LeftPress = false;

            }

            //Аналогично
            else if (e.Key == Key.D)
            {

                _RightPress = false;

            }

            //Говорим, что нужно сбросить скорость до нуля (постепенно)
            _Player.resetVelocity = true;

        }

    }

}
