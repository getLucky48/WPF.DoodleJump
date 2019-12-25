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

        public static double MaxJump;

        public double MaxVelocity;

        public static double CurrentVelocity;

        public static double MaxVelocityJump;

        public double CurrentVelocityJump;

        public Canvas canvas;

        public bool IsFalling;

        public double deltaVelocity;

        public static double deltaVelocityJump;

        public double deltaResetVelocity;

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

        public static double GetMaxJump() { return MaxJump; }

        public static double CalcMaxJump()
        {

            double tempCurrentVelocityJump = 0;

            double tempMaxJump = 0;

            while (tempCurrentVelocityJump < MaxVelocityJump)
            {

                tempCurrentVelocityJump += deltaVelocityJump;

                tempMaxJump += tempCurrentVelocityJump;

            }

            while (tempCurrentVelocityJump > 0)
            {

                tempCurrentVelocityJump -= deltaVelocityJump;

                tempMaxJump += tempCurrentVelocityJump;

            }

            return tempMaxJump;

        }

        public Player(Canvas tCanvas, double tVelocity, double tJump, Location tLocation)
        {

            MaxVelocity = tVelocity;

            MaxVelocityJump = tJump;

            deltaVelocity = MaxVelocity * 0.3;

            deltaVelocityJump = MaxVelocityJump * 0.08;

            deltaResetVelocity = MaxVelocity * 0.05;

            CurrentVelocity = 0;

            score = 0;

            canvas = tCanvas;

            canvas.Children.Insert(0, this);

            SetLocation(tLocation);

            IsFalling = false;

            resetVelocity = false;

            isAlive = true;

            MaxJump = CalcMaxJump();

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

            if (CurrentVelocity - deltaVelocity > -MaxVelocity) { CurrentVelocity -= deltaVelocity; }

        }

        public void MoveRight()
        {

            Image_Player.FlowDirection = FlowDirection.LeftToRight;

            if (CurrentVelocity + deltaVelocity < MaxVelocity) { CurrentVelocity += deltaVelocity; }

        }
        
        public void Jump()
        {

            if (!IsFalling)
            {

                if (CurrentVelocityJump + deltaVelocityJump < MaxVelocityJump) { CurrentVelocityJump += deltaVelocityJump; }

                else { IsFalling = !IsFalling; }

            }
            else {

                CurrentVelocityJump -= deltaVelocityJump;

                if ((CurrentVelocityJump <= 0) && OnCollisionEnter())
                {
                    
                    CurrentVelocityJump = 0.0;

                    IsFalling = !IsFalling;

                }

            }

        }

        public void ResetVelocity()
        {

            if(!resetVelocity) { return; }

            if (CurrentVelocity < 0)
            {

                if (CurrentVelocity > -deltaResetVelocity) { CurrentVelocity = 0; }

                else { CurrentVelocity += deltaResetVelocity; }

            }

            else if (CurrentVelocity > 0)
            {

                if (CurrentVelocity < deltaResetVelocity) { CurrentVelocity = 0; }

                else { CurrentVelocity -= deltaResetVelocity; }

            }

        }
        
        public bool OnCollisionEnterEnemy()
        {

            foreach(var target in canvas.Children.OfType<Enemy>())
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
            
            foreach (var target in canvas.Children.OfType<Platform>())
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

            if(LeftUpPoint.Y > (canvas.ActualHeight * 1.1))
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

            double xPos = LeftUpPoint.X + CurrentVelocity;

            double yPos = LeftUpPoint.Y - CurrentVelocityJump;

            if (xPos < -(canvas.ActualWidth * 0.05) ) { xPos = canvas.ActualWidth * 0.95; }

            else if (xPos > canvas.ActualWidth * 0.95) { xPos = -(canvas.ActualWidth * 0.05); }

            this.SetLocation(new Location(xPos, yPos));

        }

    }

}
