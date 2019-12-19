using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DoodleJump.Scripts
{
    class PlatformGenerator
    {

        //Метод для проверки на необходимость новых платформ
        private static bool _NeedNewPlatforms(Canvas tCanvas, Player player)
        {

            //Создаем начальную платформу
            if(tCanvas.Children.OfType<Platform>().Count() == 0)
            {

                Platform FirtsPlatform = new Platform();

                //Высчитываем центр по иксу
                double platX = (tCanvas.ActualWidth / 2) - (FirtsPlatform.Width / 2);

                //Высчитываем нижнюю координату (~ 0.05 высоты окна от низа)
                double platY = (tCanvas.ActualHeight * 0.95) - (FirtsPlatform.Height / 2);

                //Устанавливаем координату X
                FirtsPlatform.SetValue(Canvas.LeftProperty, platX);

                //Устанавливаем координату Y
                FirtsPlatform.SetValue(Canvas.TopProperty, platY);

                //Вставляем элемент на игровое поле
                tCanvas.Children.Insert(0, FirtsPlatform);

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
                if(Location.GetLocation(tCanvas.Children.OfType<Platform>().ElementAt(i)).Y > (tCanvas.ActualHeight * 1.05))
                {

                    tCanvas.Children.Remove(tCanvas.Children.OfType<Platform>().ElementAt(i));

                }

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

                double border = 10.0;

                //Задаем Y координату для новой платформы
                newPlatform.SetValue(Canvas.TopProperty, yNext);

                //Задаем X координату для новой платформы
                newPlatform.SetValue(Canvas.LeftProperty, border + (rand.Next() % ((tCanvas.ActualWidth - border) - newPlatform.Width)));

                //Добавляем новую платформу в игровую зону
                tCanvas.Children.Insert(0, newPlatform);

                //Теперь последней платформой станет та, что мы только что сделали
                locLastPlatform = Location.GetLocation(newPlatform);

            }

        }

    }

}
