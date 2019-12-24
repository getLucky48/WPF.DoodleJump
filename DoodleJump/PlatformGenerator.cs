using System;
using System.Linq;
using System.Windows.Controls;

namespace DoodleJump
{
    class PlatformGenerator
    {

        private static string[] _TypeOfPlatform = { "Platform", "PlatformBroken_1", "PlatformMoving" };

        //Метод для проверки на необходимость новых платформ
        private static bool _NeedNewPlatforms(Canvas tCanvas, Player player)
        {

            //Создаем начальную платформу
            if(tCanvas.Children.OfType<Platform>().Count() == 0)
            {

                Platform FirstPlatform = new Platform("Platform", tCanvas);

                //Высчитываем центр по иксу
                double platX = (tCanvas.ActualWidth / 2) - (FirstPlatform.Width / 2);

                //Высчитываем нижнюю координату (~ 0.05 высоты окна от низа)
                double platY = (tCanvas.ActualHeight * 0.95) - (FirstPlatform.Height / 2);

                //Задаем ширину платформы 10% от ширины окна
                FirstPlatform.Width = tCanvas.ActualWidth * 0.1;

                //Задаем высоту платформы 2% от высоты окна
                FirstPlatform.Height = tCanvas.ActualHeight * 0.02;

                //Устанавливаем координату X
                FirstPlatform.SetValue(Canvas.LeftProperty, platX);

                //Устанавливаем координату Y
                FirstPlatform.SetValue(Canvas.TopProperty, platY);

                //Вставляем элемент на игровое поле
                tCanvas.Children.Insert(0, FirstPlatform);

                //Расчитываем точки платформы
                FirstPlatform.SetLocation();

            }

            //Получаем самую последнюю созданную платформу. Хоть и написано First, но вернет последнюю, т.к. в списке
            //она будет в самом начале
            Platform lastPlatform = tCanvas.Children.OfType<Platform>().First();

            //Если игрок находится за 40% от высоты игрового поля до последней платформы, то нужно создавать новые
            if (Math.Abs(player.GetLocation().Y - Location.GetLocation(lastPlatform).Y) < (tCanvas.ActualHeight * 0.4)) { return true; }

            return false;

        }

        //Удаляет пройденные платформы
        private static void _RemoveOldPlatforms(Canvas tCanvas)
        {

            //Перебираем всю коллекцию платформ. Можно было бы использовать foreach, но выбрасывает исключение
            //из-за изменения коллекции
            for(int i = 0; i < tCanvas.Children.OfType<Platform>().Count(); i++)
            {

                //Если текущий элемент ниже окна на 5% от высоты игрового поля, то удалить
                if(tCanvas.Children.OfType<Platform>().ElementAt(i).LeftUpPoint.Y > (tCanvas.ActualHeight * 1.05))
                {

                    tCanvas.Children.Remove(tCanvas.Children.OfType<Platform>().ElementAt(i));

                }

            }

        }

        //Получаем тип платформы с определенным шансом
        private static string _GetTypeWithChance(int tChance)
        {

            tChance = tChance % 100;

            if ((0 <= tChance) && (tChance <= 20)) { return "PlatformMoving"; }

            if ((21 <= tChance) && (tChance <= 50)) { return "PlatformBroken_1"; }

            if ((51 <= tChance) && (tChance < 100)) { return "Platform"; }

            return "";

        }

        //Проверяем вхождение точки в прямоугольную зону
        public static bool PointEnter(Location tLocation, Platform tPlatform)
        {

            //Если точка находится в границах по X
            if((tPlatform.LeftUpPoint.X <= tLocation.X) && (tLocation.X <= tPlatform.RightUpPoint.X))
            {
                
                //Если точка находится в границах по Y
                if ((tPlatform.LeftUpPoint.Y <= tLocation.Y) && (tLocation.Y <= tPlatform.LeftDownPoint.Y))
                {

                    return true;

                }

                return false;

            }

            return false;

        }

        //Метод проверки пересечения новой платформы с последней
        public static bool OnCollisionEnter(Platform lastPlatform, Platform tPlatform)
        {

            //Если хоть одна точка платформы входит в другую, то есть пересечение
            if (PointEnter(tPlatform.LeftUpPoint, lastPlatform)) { return true; }

            if (PointEnter(tPlatform.LeftDownPoint, lastPlatform)) { return true; }

            if (PointEnter(tPlatform.RightUpPoint, lastPlatform)) { return true; }

            if (PointEnter(tPlatform.RightDownPoint, lastPlatform)) { return true; }

            //Если пересечений нет
            return false;

        }

        //Генерирует 20 следующих платформ
        public static void GenerateNewPlatform(Canvas tCanvas, Player player)
        {

            //_NeedNewPlatforms - мой метод. Написан для проверки на необходимость новых платформах.
            if ( !_NeedNewPlatforms(tCanvas, player) ) { return; }

            //_RemoveOldPlatforms - мой метод. Написан для удаления пройденных платформ.
            _RemoveOldPlatforms(tCanvas);

            //Получаем самую последнюю созданную платформу. Хоть и написано First, но вернет последнюю, т.к. в списке
            //она будет в самом начале
            Platform lastPlatform = tCanvas.Children.OfType<Platform>().First();
            
            //Получаем координаты последней платформы
            Location locLastPlatform = Location.GetLocation(lastPlatform);

            //Класс рандомных чисел
            Random rand = new Random();

            //Массив под 20 новых платформ
            Platform[] platforms = new Platform[20];

            //Выстраиваем список следующих платформ. Не добавляем на форму без проверки!
            for (int i = 0; i < 20; i++)
            {

                //Создаем новую платформу. Если заданной платформы не существует, то создается обычная
                Platform newPlatform = new Platform(_GetTypeWithChance(rand.Next()), tCanvas);

                //Отступ от краев окна
                double border = tCanvas.ActualWidth * 0.015;
                
                //Задаем ширину платформы 10% от ширины окна
                newPlatform.Width = tCanvas.ActualWidth * 0.1;

                //Задаем высоту платформы 2% от высоты окна
                newPlatform.Height = tCanvas.ActualHeight * 0.02;

                //Расчитывает координаты до тех пор, пока не найдет свободное место
                //То есть исключает пересечение платформ
                do
                {

                    //Рассчитываем расстояние до следующей платформы
                    double xNext = border + (rand.Next() % ((tCanvas.ActualWidth - border) - newPlatform.Width));

                    //Рассчитываем расстояние до следующей платформы
                    double yNext = locLastPlatform.Y - (rand.NextDouble() / 2 * Player.GetMaxJump());

                    //Задаем X координату для новой платформы
                    newPlatform.SetValue(Canvas.LeftProperty, xNext);

                    //Задаем Y координату для новой платформы
                    newPlatform.SetValue(Canvas.TopProperty, yNext);

                    //Расчитываем точки платформы
                    newPlatform.SetLocation();

                }
                while (OnCollisionEnter(lastPlatform, newPlatform));

                //Добавляем новую платформу в массив новых платформ
                platforms[i] = newPlatform;

                //Теперь последней платформой станет та, что мы только что сделали
                lastPlatform = newPlatform;

                //Теперь последней платформой станет та, что мы только что сделали
                locLastPlatform = Location.GetLocation(newPlatform);

            }

            //Добавляем 20 новых платформ на игровое поле
            for(int i = 0; i < 20; i++)
            {

                tCanvas.Children.Insert(0, platforms[i]);

            }

        }

    }

}
