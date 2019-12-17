using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DoodleJump.Scripts
{
    public class Location
    {

        public double X;

        public double Y;

        public Location(double tX, double tY)
        {

            X = tX;

            Y = tY;

        }

        public static Location GetLocation(UIElement ui)
        {

            Location loc = new Location( (double) ui.GetValue(Canvas.LeftProperty), (double) ui.GetValue(Canvas.TopProperty));

            return loc;

        }

    }
}
