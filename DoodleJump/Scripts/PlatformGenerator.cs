using System;
using System.Linq;
using System.Windows.Controls;

namespace DoodleJump.Scripts
{
    class PlatformGenerator
    {

        public static void GenerateNewPlatform(Canvas tCanvas, Player player)
        {

            Platform lastPlatform = tCanvas.Children.OfType<Platform>().First();

            Location locLastPlatform = Location.GetLocation(lastPlatform);

            Random rand = new Random();

            for(int i = 0; i < 20; i++)
            {

                Platform newPlatform = new Platform();

                double yNext = locLastPlatform.Y - (rand.NextDouble() * player.MaxJump * 6);

                newPlatform.SetValue(Canvas.TopProperty, yNext);

                newPlatform.SetValue(Canvas.LeftProperty, 10 + (rand.Next() % (590 - newPlatform.Width)));

                tCanvas.Children.Insert(0, newPlatform);

                locLastPlatform = Location.GetLocation(newPlatform);

            }

        }
        
        public static bool NeedNewPlatforms(Canvas tCanvas, Player player)
        {

            Platform lastPlatform = tCanvas.Children.OfType<Platform>().First();

            if (Math.Abs(player.GetLocation().Y - Location.GetLocation(lastPlatform).Y) < 300) { return true; }

            return false;

        }

    }
}
