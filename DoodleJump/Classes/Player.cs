using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DoodleJump.Classes
{

    public class Player
    {

        private Image Image_Player;

        private double StdVelocity;

        private double CurrentVelocity;

        /// <summary>
        /// Если игрок зажал клавишу A, то переменная равна -1
        /// Если игрок зажал клавишу D, то переменная равна 1
        /// Иначе 0
        /// </summary>
        public int LeftNoneRight;

        public Player(Canvas tCanvas, double tVelocity)
        {

            StdVelocity = tVelocity;

            CurrentVelocity = 0;

            LeftNoneRight = 0;

            Image_Player = CreatePlayer();

            tCanvas.Children.Insert(0, this);

            Image_Player.SetValue(Canvas.LeftProperty, 2.2);

        }

        public Image CreatePlayer()
        {

            Image temp = new Image();

            temp.Name = "Image_Player";

            temp.Height = 50;

            temp.Width = 50;
            
            temp.Source = BitmapFrame.Create(new Uri("C:\\Users\\1162262\\source\\repos\\DoodleJump\\DoodleJump\\Sprites\\Player.png", UriKind.Absolute));

            temp.Stretch = Stretch.Fill;

            return temp;

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

}
