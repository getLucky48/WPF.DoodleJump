using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DoodleJump
{

    public partial class Window_Game : Window
    {
        
        private double _Width = SystemParameters.PrimaryScreenWidth * 0.3;

        private double _Height = SystemParameters.PrimaryScreenHeight * 0.7;

        private void _AcceptWindowSettings()
        {
            
            this.Width = _Width;
            
            this.Height = _Height;
            
            this.Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);
            
            this.Left = (SystemParameters.PrimaryScreenWidth / 2) - (this.Width / 2);

        }

        public Window_Game() { InitializeComponent(); }

        private DispatcherTimer _Timer;

        private Player _Player;

        private bool _LeftPress = false;

        private bool _RightPress = false;

        private void _TimerTick(object sender, EventArgs e)
        {

            if(_Player == null ) { return; }

            if(!_Player.isAlive)
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

            LevelGenerator.GenerateNewPlatform(Canvas_GameMap, _Player);
            
        }

        private void _UpdateScore() { this.Title = "DoodleJump | Score: " + _Player.score; }

        private void _StartGame()
        {

            _Timer = new DispatcherTimer();

            _Timer.Tick += new EventHandler(_TimerTick);

            _Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            
            _Timer.Start();
            
            double playerX = (Canvas_GameMap.ActualWidth / 2);
            
            double playerY = (Canvas_GameMap.ActualHeight * 0.95);
            
            _Player = new Player(Canvas_GameMap, _Width * 0.015, _Height * 0.02, new Location(playerX, playerY));
            
            _Player.Width = Canvas_GameMap.ActualWidth * 0.1;
            
            _Player.Height = Canvas_GameMap.ActualHeight * 0.07;

            LevelGenerator.CreateStartPlatform(Canvas_GameMap);

        }
        
        private void _Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            _AcceptWindowSettings();
            
            _StartGame();

        }
        
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
        
        private void _Window_KeyDown(object sender, KeyEventArgs e)
        {
            
            if(e.Key == Key.A) { _LeftPress = true; }
            
            else if (e.Key == Key.D) { _RightPress = true; }

        }
        
        private void _Window_KeyUp(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.A) { _LeftPress = false; }
            
            else if (e.Key == Key.D) { _RightPress = false; }

            _Player.resetVelocity = true;

        }

    }

}
