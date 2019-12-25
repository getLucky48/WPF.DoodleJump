using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DoodleJump
{

    public partial class Platform : UserControl
    {

        public Canvas _Canvas;

        public Location LeftUpPoint;

        public Location LeftDownPoint;

        public Location RightUpPoint;

        public Location RightDownPoint;

        public Platform(string tType, Canvas tCanvas)
        {

            InitializeComponent();

            SetTypePlatform(tType);

            MovingLeft = false;

            deltaVelocity = 1.0;

            _Canvas = tCanvas;

        }

        public string Type;

        public bool MovingLeft;

        public double deltaVelocity;

        public DispatcherTimer Timer;

        public void SetTypePlatform(string tType)
        {
            
            if(Type == null) { Type = "Platform"; }

            string NewImage = Image_Platform.Source.ToString().Replace(Type, tType);

            try { Image_Platform.Source = new BitmapImage(new Uri(NewImage)); }

            catch (IOException) { return; };

            Type = tType;

        }

        public string GetTypePlatform() { return Type; }

        public void UpdateLocation()
        {

            LeftUpPoint = Location.GetLocation(this);

            LeftDownPoint = new Location(LeftUpPoint.X, LeftUpPoint.Y + this.Height);

            RightUpPoint = new Location(LeftUpPoint.X + this.Width, LeftUpPoint.Y);

            RightDownPoint = new Location(LeftUpPoint.X + this.Width, LeftUpPoint.Y + this.Height);

        }

        private void _UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (Type.Contains("PlatformMoving"))
            {

                Timer = new DispatcherTimer();

                Timer.Tick += new EventHandler(MovePlatform);

                Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

                Timer.Start();

            }

        }

        public void MovePlatform(object sender, EventArgs e)
        {

            if (MovingLeft)
            {

                if (LeftUpPoint.X < _Canvas.ActualWidth * 0.05) { MovingLeft = !MovingLeft; }

                else
                {

                    this.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) - deltaVelocity);

                }

            }

            else
            {

                if (RightUpPoint.X > _Canvas.ActualWidth * 0.95) { MovingLeft = !MovingLeft; }

                else
                {

                    this.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) + deltaVelocity);

                }

            }

            this.UpdateLocation();

        }
        
        public async Task BreakPlatformAsync()
        {

            if (Type.Contains("PlatformBroken_1"))
            {

                string NewImage = Image_Platform.Source.ToString().Replace(Type, "PlatformBroken_2");

                Image_Platform.Source = new BitmapImage(new Uri(NewImage));

                this.Height = Image_Platform.Height;

                await Task.Delay(500);

                this.Visibility = Visibility.Hidden;

            }

        }

    }

}
