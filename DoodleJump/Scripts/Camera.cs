using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoodleJump.Scripts
{
    class Camera
    {

        //Перемещает "камеру"
        public static void TransformCamera(Canvas tCanvas, Player player)
        {

            //Если игрок достиг определенной высоты (40% до шапки окна от общей высоты игрового поля), 
            //то передвинуть игрока и все платформы
            if (player.GetLocation().Y < (tCanvas.ActualHeight * 0.4))
            {

                //Просчитываем смещение
                double translation = (tCanvas.ActualHeight * 0.4) - player.GetLocation().Y;

                //Добавляем очки игроку
                player.score += (long) translation;

                //Задаем новые координаты Y игроку
                player.SetValue(Canvas.TopProperty, (double)player.GetValue(Canvas.TopProperty) + translation);

                //Задаем новые координаты Y всем платформам
                foreach (var target in tCanvas.Children.OfType<Platform>())
                {

                    target.SetValue(Canvas.TopProperty, (double)target.GetValue(Canvas.TopProperty) + translation);

                }

            }

        }

    }

}
