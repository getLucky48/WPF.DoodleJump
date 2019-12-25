using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DoodleJump
{

    public partial class Enemy : UserControl
    {

        public Enemy(Canvas tCanvas)
        {

            InitializeComponent();

            MovingLeft = false;

            deltaVelocity = 3.0;

            canvas = tCanvas;

            this.Width = canvas.ActualWidth * 0.1;

            this.Height = canvas.ActualHeight * 0.07;

        }

        public Canvas canvas;

        public Location LeftUpPoint;

        public Location LeftDownPoint;

        public Location RightUpPoint;

        public Location RightDownPoint;

        public bool MovingLeft;

        public double deltaVelocity;

        public DispatcherTimer _Timer;

        public void UpdateLocation()
        {

            LeftUpPoint = Location.GetLocation(this);

            LeftDownPoint = new Location(LeftUpPoint.X, LeftUpPoint.Y + this.Height);

            RightUpPoint = new Location(LeftUpPoint.X + this.Width, LeftUpPoint.Y);

            RightDownPoint = new Location(LeftUpPoint.X + this.Width, LeftUpPoint.Y + this.Height);

        }

        public void MoveEnemy(object sender, EventArgs e)
        {

            if (MovingLeft)
            {

                if (LeftUpPoint.X < canvas.ActualWidth * 0.05)
                {

                    MovingLeft = !MovingLeft;

                    Image_Enemy.FlowDirection = FlowDirection.LeftToRight;

                }

                else
                {

                    this.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) - deltaVelocity);

                }

            }

            else
            {

                if (RightUpPoint.X > canvas.ActualWidth * 0.95)
                {

                    MovingLeft = !MovingLeft;

                    Image_Enemy.FlowDirection = FlowDirection.RightToLeft;

                }

                else
                {

                    this.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) + deltaVelocity);

                }

            }

            this.UpdateLocation();

        }

        private void _UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.UpdateLocation();
            
            _Timer = new DispatcherTimer();

            _Timer.Tick += new EventHandler(MoveEnemy);
            
            _Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            
            _Timer.Start();

        }
        
        public bool PointEnter(Location tLocation)
        {
            
            if ((LeftUpPoint.X <= tLocation.X) && (tLocation.X <= RightUpPoint.X))
                
                if ((LeftUpPoint.Y <= tLocation.Y) && (tLocation.Y <= LeftDownPoint.Y)) { return true; }

            return false;

        }

        public bool OnCollisionEnter(Player tPlayer)
        {

            if (PointEnter(tPlayer.LeftUpPoint)) { return true; }

            if (PointEnter(tPlayer.LeftDownPoint)) { return true; }

            if (PointEnter(tPlayer.RightUpPoint)) { return true; }

            if (PointEnter(tPlayer.RightDownPoint)) { return true; }
            
            return false;

        }

    }

}
