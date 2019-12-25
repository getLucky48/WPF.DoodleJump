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

        public void _AcceptWindowSettings()
        {
            
            this.Width = _Width;
            
            this.Height = _Height;
            
            this.Top = (SystemParameters.PrimaryScreenHeight / 2) - (this.Height / 2);
            
            this.Left = (SystemParameters.PrimaryScreenWidth / 2) - (this.Width / 2);

        }

        public Window_Game() { InitializeComponent(); }

        public DispatcherTimer Timer;

        public Player player;

        public bool LeftPress = false;

        public bool RightPress = false;

        public void TimerTick(object sender, EventArgs e)
        {

            if(player == null ) { return; }

            if(!player.isAlive)
            {

                Timer.Stop();

                Window_Gameover window_Gameover = new Window_Gameover(player.score);

                window_Gameover.Show();

                this.Close();

                return;

            }

            MovePlayer();

            player.ChangePlayerPosition();

            UpdateScore();

            Camera.TransformCamera(Canvas_GameMap, player);

            LevelGenerator.GenerateNewPlatform(Canvas_GameMap, player);
            
        }

        public void UpdateScore() { this.Title = "DoodleJump | Score: " + player.score; }

        public void StartGame()
        {

            Timer = new DispatcherTimer();

            Timer.Tick += new EventHandler(TimerTick);

            Timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            
            Timer.Start();
            
            double playerX = (Canvas_GameMap.ActualWidth / 2);
            
            double playerY = (Canvas_GameMap.ActualHeight * 0.95);
            
            player = new Player(Canvas_GameMap, _Width * 0.015, _Height * 0.02, new Location(playerX, playerY));
            
            player.Width = Canvas_GameMap.ActualWidth * 0.1;
            
            player.Height = Canvas_GameMap.ActualHeight * 0.07;

            LevelGenerator.CreateStartPlatform(Canvas_GameMap);

        }

        public void MovePlayer()
        {

            if (LeftPress)
            {

                player.resetVelocity = false;

                player.MoveLeft();

            }

            else if (RightPress)
            {

                player.resetVelocity = false;

                player.MoveRight();

            }

        }

        private void _Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            _AcceptWindowSettings();
            
            StartGame();

        }
        
        private void _Window_KeyDown(object sender, KeyEventArgs e)
        {
            
            if(e.Key == Key.A) { LeftPress = true; }
            
            else if (e.Key == Key.D) { RightPress = true; }

        }
        
        private void _Window_KeyUp(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.A) { LeftPress = false; }
            
            else if (e.Key == Key.D) { RightPress = false; }

            player.resetVelocity = true;

        }

    }

}
