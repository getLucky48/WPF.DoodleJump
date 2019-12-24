using System.Windows;
using System.Windows.Controls;

namespace DoodleJump
{
    public class Location
    {

        //Ось x
        public double X;

        //Ось Y
        public double Y;

        //Конструктор класса
        public Location(double tX, double tY)
        {

            X = tX;

            Y = tY;

        }

        //Получить координаты элемента
        public static Location GetLocation(UIElement ui)
        {

            Location loc = new Location( (double) ui.GetValue(Canvas.LeftProperty), (double) ui.GetValue(Canvas.TopProperty));

            return loc;

        }

    }

}
