using System;
using System.Linq;
using System.Windows.Controls;

namespace DoodleJump.Scripts
{
    class PlatformGenerator
    {
        //300 700
        //Метод для проверки на необходимость новых платформ
        private static bool _NeedNewPlatforms(Canvas tCanvas, Player player)
        {

            //Создаем начальную платформу
            if(tCanvas.Children.OfType<Platform>().Count() == 0)
            {

                Platform plat = new Platform();

                plat.SetValue(Canvas.LeftProperty, 250.0);

                plat.SetValue(Canvas.TopProperty, 725.0);

                tCanvas.Children.Insert(0, plat);

            }

            //Получаем самую последнюю созданную платформу. Хоть и написано First, но вернет последнюю, т.к. в списке
            //она будет в самом начале
            Platform lastPlatform = tCanvas.Children.OfType<Platform>().First();

            //Если игрок находится за 300 пикселей до последней платформы, то нужно создавать новые
            if (Math.Abs(player.GetLocation().Y - Location.GetLocation(lastPlatform).Y) < 300) { return true; }

            return false;

        }

        //Удаляет пройденные платформы
        private static void _RemoveOldPlatforms(Canvas tCanvas)
        {

            //Перебираем всю коллекцию платформ. Можно было бы использовать foreach, но выбрасывает исключение
            //из-за изменения коллекции
            for(int i = 0; i < tCanvas.Children.OfType<Platform>().Count(); i++)
            {

                //Если текущий элемент ниже окна (ниже 800 пикселей), то удалить
                if(Location.GetLocation(tCanvas.Children.OfType<Platform>().ElementAt(i)).Y > 900)
                {

                    tCanvas.Children.Remove(tCanvas.Children.OfType<Platform>().ElementAt(i));

                }

            }

        }

        //Удаляет пройденные платформы
        public static void RemoveAllPlatforms(Canvas tCanvas)
        {

            for (int i = 0; i < tCanvas.Children.OfType<Platform>().Count(); i++)
            {

                tCanvas.Children.Remove(tCanvas.Children.OfType<Platform>().ElementAt(i));

            }

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

            for(int i = 0; i < 20; i++)
            {

                Platform newPlatform = new Platform();

                //Рассчитываем расстояние до следующей платформы
                double yNext = locLastPlatform.Y - (rand.NextDouble() * player.GetMaxJump() * 6);

                //Задаем Y координату для новой платформы
                newPlatform.SetValue(Canvas.TopProperty, yNext);

                //Задаем X координату для новой платформы
                newPlatform.SetValue(Canvas.LeftProperty, 10 + (rand.Next() % (590 - newPlatform.Width)));

                //Добавляем новую платформу в игровую зону
                tCanvas.Children.Insert(0, newPlatform);

                //Теперь последней платформой станет та, что мы только что сделали
                locLastPlatform = Location.GetLocation(newPlatform);

            }

        }

    }

}
