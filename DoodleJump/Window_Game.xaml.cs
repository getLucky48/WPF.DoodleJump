
using DoodleJump.Scripts;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DoodleJump
{

    public partial class Window_Game : Window
    {

        public Window_Game() { InitializeComponent(); }

        private DispatcherTimer timer;

        private Player player; 

        private void TimerTick(object sender, EventArgs e)
        {

            player.ChangePlayerPosition();

            Camera.TransformCamera(Canvas_GameMap, player);

            if(PlatformGenerator.NeedNewPlatforms(Canvas_GameMap, player))
            {

                PlatformGenerator.GenerateNewPlatform(Canvas_GameMap, player);

            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            timer = new DispatcherTimer();

            timer.Tick += new EventHandler(TimerTick);

            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            timer.Start();

            player = new Player(Canvas_GameMap, 10.0, 15.0, new Location(250.0, 650.0));

            PlatformGenerator.GenerateNewPlatform(Canvas_GameMap, player);

        }
               
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.A)
            {

                player.resetVelocity = false;

                player.MoveLeft();

            }

            if (e.Key == Key.D)
            {

                player.resetVelocity = false;

                player.MoveRight();

            }

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

            player.resetVelocity = true;

        }
    }
}
