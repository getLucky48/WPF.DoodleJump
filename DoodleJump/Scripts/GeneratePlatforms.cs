using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DoodleJump.Scripts
{
    class GeneratePlatforms
    {

        public static void CheckPlatforms(Canvas tCanvas, Player player)
        {

            

            if(tCanvas.Children.OfType<Platform>().Count<Platform>() == 1)
            {

                Location locationOfFirstPlatform = Location.GetLocation(tCanvas.Children.OfType<Platform>().First<Platform>());



            }

            if(player.GetLocation().Y < 300)
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
