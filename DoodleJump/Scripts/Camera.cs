using System.Linq;
using System.Windows.Controls;

namespace DoodleJump.Scripts
{
    class Camera
    {

        public static void TransformCamera(Canvas tCanvas, Player player)
        {

            if (player.GetLocation().Y < 300)
            {

                double translation = 300 - player.GetLocation().Y;

                player.SetValue(Canvas.TopProperty, (double)player.GetValue(Canvas.TopProperty) + translation);

                foreach (var target in tCanvas.Children.OfType<Platform>())
                {

                    target.SetValue(Canvas.TopProperty, (double)target.GetValue(Canvas.TopProperty) + translation);

                }

            }

        }

    }
}
