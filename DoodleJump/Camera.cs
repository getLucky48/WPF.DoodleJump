using System.Linq;
using System.Windows.Controls;

namespace DoodleJump
{
    class Camera
    {
        
        public static void TransformCamera(Canvas tCanvas, Player player)
        {

            if (player.LeftUpPoint.Y < (tCanvas.ActualHeight * 0.4))
            {
                
                double translation = (tCanvas.ActualHeight * 0.4) - player.LeftUpPoint.Y;
                
                player.score += (long) translation;
                
                player.SetValue(Canvas.TopProperty, (double)player.GetValue(Canvas.TopProperty) + translation);

                player.UpdateLocation();
                
                foreach (var target in tCanvas.Children.OfType<Platform>())
                {

                    target.SetValue(Canvas.TopProperty, (double)target.GetValue(Canvas.TopProperty) + translation);

                    target.UpdateLocation();

                }

                foreach (var target in tCanvas.Children.OfType<Enemy>())
                {

                    target.SetValue(Canvas.TopProperty, (double)target.GetValue(Canvas.TopProperty) + translation);

                    target.UpdateLocation();

                }

            }

        }

    }

}
