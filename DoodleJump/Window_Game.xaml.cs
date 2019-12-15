
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
    /*
    public class Player : Window_Game
    {

        private double StdVelocity;

        private double CurrentVelocity;

        /// <summary>
        /// Если игрок зажал клавишу A, то переменная равна -1
        /// Если игрок зажал клавишу D, то переменная равна 1
        /// Иначе 0
        /// </summary>
        public int LeftNoneRight;

        public Player(double tVelocity)
        {

            StdVelocity = tVelocity;

            CurrentVelocity = 0;

            LeftNoneRight = 0;

        }

        public void MoveLeft()
        {

            Image_Player.FlowDirection = FlowDirection.RightToLeft;

            CurrentVelocity = -StdVelocity;

        }

        public void MoveRight()
        {

            Image_Player.FlowDirection = FlowDirection.LeftToRight;

            CurrentVelocity = StdVelocity;

        }

        public void ChangePlayerPosition()
        {

            double xPos = (double)Image_Player.GetValue(Canvas.LeftProperty) + CurrentVelocity;

            if (xPos < -35)
            {

                xPos = 565;

            }

            else if (xPos > 565)
            {

                xPos = -35;

            }

            Image_Player.SetValue(Canvas.LeftProperty, xPos);

        }

    }

    */
    /// <summary>
    /// ///////////////////////////Решить вопрос с НЕактивностью окна
    /// </summary>
    public partial class Window_Game : Window
    {

        public Window_Game() { InitializeComponent(); }

        private DispatcherTimer timer;

        Player player; 

        private void TimerTick(object sender, EventArgs e)
        {

            player.ChangePlayerPosition();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            timer = new DispatcherTimer();

            timer.Tick += new EventHandler(TimerTick);

            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            timer.Start();

            player = new Player(Canvas_GameMap, 2.5, new Location(300.0, 200.0));  

        }
               
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {

                player.MoveLeft();

            }

            if (e.Key == Key.D)
            {

                player.MoveRight();

            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {



        }
    }
}
