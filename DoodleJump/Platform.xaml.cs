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

        private Canvas _Canvas;

        public Location LeftUpPoint;

        public Location LeftDownPoint;

        public Location RightUpPoint;

        public Location RightDownPoint;

        public Platform(string tType, Canvas tCanvas)
        {

            InitializeComponent();

            SetTypePlatform(tType);

            _MovingLeft = false;

            _deltaVelocity = 1.0;

            _Canvas = tCanvas;

        }

        private string _Type;

        private bool _MovingLeft;

        private double _deltaVelocity;

        private DispatcherTimer _Timer;

        public void SetTypePlatform(string tType)
        {
            
            if(_Type == null) { _Type = "Platform"; }

            string NewImage = Image_Platform.Source.ToString().Replace(_Type, tType);

            try { Image_Platform.Source = new BitmapImage(new Uri(NewImage)); }

            catch (IOException) { return; };

            _Type = tType;

        }

        public string GetTypePlatform() { return _Type; }

        public void UpdateLocation()
        {

            LeftUpPoint = Location.GetLocation(this);

            LeftDownPoint = new Location(LeftUpPoint.X, LeftUpPoint.Y + this.Height);

            RightUpPoint = new Location(LeftUpPoint.X + this.Width, LeftUpPoint.Y);

            RightDownPoint = new Location(LeftUpPoint.X + this.Width, LeftUpPoint.Y + this.Height);

        }

        private void _UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (_Type.Contains("PlatformMoving"))
            {

                _Timer = new DispatcherTimer();

                _Timer.Tick += new EventHandler(_MovePlatform);

                _Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

                _Timer.Start();

            }

        }

        private void _MovePlatform(object sender, EventArgs e)
        {

            if (_MovingLeft)
            {

                if (LeftUpPoint.X < _Canvas.ActualWidth * 0.05) { _MovingLeft = !_MovingLeft; }

                else
                {

                    this.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) - _deltaVelocity);

                }

            }

            else
            {

                if (RightUpPoint.X > _Canvas.ActualWidth * 0.95) { _MovingLeft = !_MovingLeft; }

                else
                {

                    this.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) + _deltaVelocity);

                }

            }

            this.UpdateLocation();

        }
        
        public async Task BreakPlatformAsync()
        {

            if (_Type.Contains("PlatformBroken_1"))
            {

                string NewImage = Image_Platform.Source.ToString().Replace(_Type, "PlatformBroken_2");

                Image_Platform.Source = new BitmapImage(new Uri(NewImage));

                this.Height = Image_Platform.Height;

                await Task.Delay(500);

                this.Visibility = Visibility.Hidden;

            }

        }

    }

}
