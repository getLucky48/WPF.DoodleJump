using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoodleJump
{

    public partial class Player : UserControl
    {

        public Location LeftUpPoint;

        public Location LeftDownPoint;

        public Location RightUpPoint;

        public Location RightDownPoint;

        private static double _MaxJump;

        private double _MaxVelocity;

        private static double _CurrentVelocity;

        private static double _MaxVelocityJump;

        private double _CurrentVelocityJump;

        private Canvas _ParentCanvas;

        private bool _IsFalling;

        private double _deltaVelocity;

        private static double _deltaVelocityJump;

        private double _deltaResetVelocity;

        public bool resetVelocity;

        public bool isAlive;

        public long score;

        public void UpdateLocation()
        {

            LeftUpPoint = Location.GetLocation(this);

            LeftDownPoint = new Location(LeftUpPoint.X, LeftUpPoint.Y + this.Height);

            RightUpPoint = new Location(LeftUpPoint.X + this.Width, LeftUpPoint.Y);

            RightDownPoint = new Location(LeftUpPoint.X + this.Width, LeftUpPoint.Y + this.Height);

        }

        public static double GetMaxJump() { return _MaxJump; }

        public static double CalcMaxJump()
        {

            double tempCurrentVelocityJump = 0;

            double tempMaxJump = 0;

            while (tempCurrentVelocityJump < _MaxVelocityJump)
            {

                tempCurrentVelocityJump += _deltaVelocityJump;

                tempMaxJump += tempCurrentVelocityJump;

            }

            while (tempCurrentVelocityJump > 0)
            {

                tempCurrentVelocityJump -= _deltaVelocityJump;

                tempMaxJump += tempCurrentVelocityJump;

            }

            return tempMaxJump;

        }

        public Player(Canvas tCanvas, double tVelocity, double tJump, Location tLocation)
        {

            _MaxVelocity = tVelocity;

            _MaxVelocityJump = tJump;

            _deltaVelocity = _MaxVelocity * 0.3;

            _deltaVelocityJump = _MaxVelocityJump * 0.08;

            _deltaResetVelocity = _MaxVelocity * 0.05;

            _CurrentVelocity = 0;

            score = 0;

            _ParentCanvas = tCanvas;

            _ParentCanvas.Children.Insert(0, this);

            SetLocation(tLocation);

            _IsFalling = false;

            resetVelocity = false;

            isAlive = true;

            _MaxJump = CalcMaxJump();

            InitializeComponent();

            this.UpdateLocation();

        }

        public void SetLocation(Location tLocation)
        {
            
            this.SetValue(Canvas.LeftProperty, tLocation.X);
            
            this.SetValue(Canvas.TopProperty, tLocation.Y);

            this.UpdateLocation();

        }

        public void MoveLeft()
        {

            Image_Player.FlowDirection = FlowDirection.RightToLeft;

            if (_CurrentVelocity - _deltaVelocity > -_MaxVelocity) { _CurrentVelocity -= _deltaVelocity; }

        }

        public void MoveRight()
        {

            Image_Player.FlowDirection = FlowDirection.LeftToRight;

            if (_CurrentVelocity + _deltaVelocity < _MaxVelocity) { _CurrentVelocity += _deltaVelocity; }

        }
        
        public void Jump()
        {

            if (!_IsFalling)
            {

                if (_CurrentVelocityJump + _deltaVelocityJump < _MaxVelocityJump) { _CurrentVelocityJump += _deltaVelocityJump; }

                else { _IsFalling = !_IsFalling; }

            }
            else {

                _CurrentVelocityJump -= _deltaVelocityJump;

                if ((_CurrentVelocityJump <= 0) && OnCollisionEnter())
                {
                    
                    _CurrentVelocityJump = 0.0;

                    _IsFalling = !_IsFalling;

                }

            }

        }

        public void ResetVelocity()
        {

            if(!resetVelocity) { return; }

            if (_CurrentVelocity < 0)
            {

                if (_CurrentVelocity > -_deltaResetVelocity) { _CurrentVelocity = 0; }

                else { _CurrentVelocity += _deltaResetVelocity; }

            }

            else if (_CurrentVelocity > 0)
            {

                if (_CurrentVelocity < _deltaResetVelocity) { _CurrentVelocity = 0; }

                else { _CurrentVelocity -= _deltaResetVelocity; }

            }

        }
        
        public bool OnCollisionEnterEnemy()
        {

            foreach(var target in _ParentCanvas.Children.OfType<Enemy>())
            {

                if (target.OnCollisionEnter(this)) { return true; }

            }

            return false;

        }

        public static bool PointEnter(Location tLocation, Platform tPlatform)
        {

            if ((tPlatform.LeftUpPoint.X <= tLocation.X) && (tLocation.X <= tPlatform.RightUpPoint.X)) {

                if ((tPlatform.LeftUpPoint.Y <= tLocation.Y) && (tLocation.Y <= tPlatform.LeftDownPoint.Y + 10.0)) { return true; }
 }
            return false;

        }

        public bool OnCollisionEnter()
        {
            
            foreach (var target in _ParentCanvas.Children.OfType<Platform>())
            {

                if (PointEnter(this.LeftDownPoint, target))
                {

                    if (target.GetTypePlatform().Contains("PlatformBroken"))
                    {

                        target.BreakPlatformAsync();

                        return false;

                    }
                
                    return true;

                }

                if (PointEnter(this.RightDownPoint, target))
                {
                
                    if (target.GetTypePlatform().Contains("PlatformBroken"))
                    {

                        target.BreakPlatformAsync();

                        return false;

                    }

                    return true;

                }

            }

            return false;

        }

        public void ChangePlayerPosition()
        {

            if(LeftUpPoint.Y > (_ParentCanvas.ActualHeight * 1.1))
            {

                isAlive = false;

                return;

            }

            if (OnCollisionEnterEnemy())
            {

                isAlive = false;

                return;

            }

            Jump();

            ResetVelocity();

            double xPos = LeftUpPoint.X + _CurrentVelocity;

            double yPos = LeftUpPoint.Y - _CurrentVelocityJump;

            if (xPos < -(_ParentCanvas.ActualWidth * 0.05) ) { xPos = _ParentCanvas.ActualWidth * 0.95; }

            else if (xPos > _ParentCanvas.ActualWidth * 0.95) { xPos = -(_ParentCanvas.ActualWidth * 0.05); }

            this.SetLocation(new Location(xPos, yPos));

        }

    }

}
