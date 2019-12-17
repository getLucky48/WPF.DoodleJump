
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

            GeneratePlatforms.CheckPlatforms(Canvas_GameMap, player);
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            timer = new DispatcherTimer();

            timer.Tick += new EventHandler(TimerTick);

            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            timer.Start();

            player = new Player(Canvas_GameMap, 10.0, 15.0, new Location(250.0, 650.0));  
            ////
            ///

            Platform lastPl = Canvas_GameMap.Children.OfType<Platform>().First<Platform>();

            Location locPl = Location.GetLocation(lastPl);

            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {

                Platform newPl = new Platform();

                double nextY = locPl.Y - (rand.NextDouble() * player.MaxJump * 7);

                newPl.SetValue(Canvas.TopProperty, nextY);

                newPl.SetValue(Canvas.LeftProperty, (double)(rand.Next() % 400));

                Canvas_GameMap.Children.Insert(0, newPl);

                locPl = Location.GetLocation(newPl);

            }

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
