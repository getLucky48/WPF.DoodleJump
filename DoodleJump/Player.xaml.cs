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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoodleJump
{

    public partial class Player : UserControl
    {

        public Player()
        {
            InitializeComponent();
        }

        private double StdVelocity;

        private double CurrentVelocity;

        /// <summary>
        /// Если игрок зажал клавишу A, то переменная равна -1;
        /// Если игрок зажал клавишу D, то переменная равна 1;
        /// Иначе 0;
        /// </summary>
        public int LeftNoneRight;

        public Player(Canvas tCanvas, double tVelocity, Location tLocation)
        {

            StdVelocity = tVelocity;

            CurrentVelocity = 0;

            LeftNoneRight = 0;

            Image_Player = SetImage();

            tCanvas.Children.Insert(0, this);

            SetLocation(this, tLocation);

            InitializeComponent();

        }

        public void SetLocation(UIElement ui, Location tLocation)
        {

            ui.SetValue(Canvas.LeftProperty, tLocation.X);

            ui.SetValue(Canvas.TopProperty, tLocation.Y);

        }

        public Image SetImage()
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

            double xPos = (double)this.GetValue(Canvas.LeftProperty) + CurrentVelocity;

            if (xPos < -35)
            {

                xPos = 565;

            }

            else if (xPos > 565)
            {

                xPos = -35;

            }

            this.SetValue(Canvas.LeftProperty, xPos);

        }
    }
}
