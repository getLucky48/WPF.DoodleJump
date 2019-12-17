using DoodleJump.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoodleJump
{

    public partial class Player : UserControl
    {

        //Максимальная скорость игрока по сторонам
        private double MaxVelocity;

        //Текущая скорость игрока по сторонам. Нужно для плавного передвижения
        private double CurrentVelocity;

        //Максимальная высота прыжка
        public double MaxJump;

        //Текущая высота прыжка. Нужно для плавного подъема и спуска
        public double CurrentJump;

        //Канвас - на чем рисуем и размещаем объекты. Необходимо для проверки координат платформ
        Canvas ParentCanvas;

        //Если true - значит, игрок падает, и мы ищем взаимодействие с платформой для следующего прыжка
        private bool IsFalling;

        //Если true - начинаем снижение скорости
        public bool resetVelocity;

        //Конструктор класса. Создаем игрока, задаем канвас для размещения картинки, макс. скорость, макс. прыжок и локацию
        //Location - мой класс, хранит два double-значения X, Y и парочку методов
        public Player(Canvas tCanvas, double tVelocity, double tJump, Location tLocation)
        {

            MaxVelocity = tVelocity;
            MaxJump = tJump;

            CurrentVelocity = 0;

            ParentCanvas = tCanvas;

            //Добавляем на канвас нашего персонажа
            ParentCanvas.Children.Insert(0, this);

            //SetLocation - мой метод. Описан ниже. Задает локацию для [игрока this]
            SetLocation(this, tLocation);

            IsFalling = false;
            resetVelocity = false;

            //Инициализируем компонент. Без него ничего не работает. Это стандартный метод для любого элемента.
            InitializeComponent();

        }

        //Задаем локацию для элемента ui; ui - элемент управления wpf, работает в том числе и для Player, т.к. он
        //является составным элементом управления (наследование от UserControl)
        public void SetLocation(UIElement ui, Location tLocation)
        {

            //Устанавливаем X координату игрока. Отсчет от левой стороны окна.
            ui.SetValue(Canvas.LeftProperty, tLocation.X);

            //Устанавливаем Y координату игрока. Отсчет от шапки окна. Да, неудобно :3
            ui.SetValue(Canvas.TopProperty, tLocation.Y);

        }

        //Метод, возвращающий расположение игрока
        public Location GetLocation()
        {

            return new Location((double)this.GetValue(Canvas.LeftProperty), (double)this.GetValue(Canvas.TopProperty));

        }

        /// <summary>
        /// 
        /// Движение персонажа влево. Вызывается каждым нажатием клавиши A на клавиатуре.
        /// При движении влево картинка отзеркаливается, чтобы персонаж смотрел в сторону движения.
        /// 
        /// Смысл метода заключается в том, чтобы добавлять скорость. В зависимости от знака скорости
        /// определяется его направление.
        /// 
        /// </summary>
        public void MoveLeft()
        {

            Image_Player.FlowDirection = FlowDirection.RightToLeft;

            if (CurrentVelocity - 3.0 > -MaxVelocity) { CurrentVelocity -= 3.0; }

        }

        //Движение вправо. Аналогично движению влево
        public void MoveRight()
        {

            Image_Player.FlowDirection = FlowDirection.LeftToRight;

            if (CurrentVelocity + 3.0 < MaxVelocity) { CurrentVelocity += 3.0; }

        }


        // Метод прыжка. Персонаж должен все время прыгать, поэтому метод вызывается каждую 1 мс.
        public void Jump()
        {

            //Если игрок не падает, значит двигается вверх
            if (!IsFalling)
            {

                //Если текущая высота прыжка меньше максимальной, то увеличиваем
                //Иначе, говорим, что игрок достиг максимальной высоты и начинает падать
                if (CurrentJump + 1.0 < MaxJump) { CurrentJump += 1.0; }
                else { IsFalling = !IsFalling; }

            }
            //Если игрок падает
            else {
                
                //Уменьшаем текущую "высоту" прыжка. Может уйти в минус - нормально.
                //Будет уменьшаться до тех пор, пока не упадет на платформу, иначе - будет бесконечно падать.
                CurrentJump -= 1.0;

                //OnCollisionEnter - мой метод. Проверяет соприкасается ли объект с платформами.
                //true - если упал на платформу
                if (OnCollisionEnter(this))
                {

                    //Если упал на платформу, то текущая высота прыжка равна нулю и персонаж на какой-то момент остановится
                    CurrentJump = 0.0;

                    //Если упал на платформу, значит уже не падает :D
                    IsFalling = !IsFalling;

                }

            }

        }

        //При отпуске клавиш управления A и D скорость уменьшается до нуля постепенно
        public void ResetVelocity()
        {

            //Если движение влево
            if (CurrentVelocity < 0)
            {

                //По умолчанию я уменьшаю на 0.25. Вдруг, текущая скорость 0.1, то мы в ноль не попадем, поэтому такое ветвление
                if (CurrentVelocity > -0.25) { CurrentVelocity = 0; }

                else { CurrentVelocity += 0.25; }

            }

            //Если движение вправо
            else if (CurrentVelocity > 0)
            {

                //По умолчанию я уменьшаю на 0.25. Вдруг, текущая скорость 0.1, то мы в ноль не попадем, поэтому такое ветвление
                if (CurrentVelocity < 0.25) { CurrentVelocity = 0; }

                else { CurrentVelocity -= 0.25; }

            }

        }

        //Метод проверки столкновений. ui проверяемый элемент. В данном проекте - игрок.
        public bool OnCollisionEnter(UIElement ui)
        {

            //Получаем список всех дочерних элементов канваса - платформы
            //Затем перебираем в foreach
            foreach(var target in ParentCanvas.Children.OfType<Platform>())
            {

                //Координаты платформы
                double x_OfPlatformLeft = (double)Location.GetLocation(target).X;
                double y_OfPlatformLeft = (double)Location.GetLocation(target).Y;

                //Координаты игрока
                double x_OfuiLeft = (double)Location.GetLocation(ui).X;
                double y_OfuiLeft = (double)Location.GetLocation(ui).Y;

                /*
                 * 
                 * Тут лучше на примере. Пусть Y платформы 60, высота персонажа 50,  Y персонажа 100
                 * 
                 * Необходимо учитывать высоту персонажа, так как отсчет всех элементов идет с верхнего левого угла,
                 * а нам нужно получить взаимодействие с нижней частью персонажа.
                 * 
                 * Тогда, разница между Y платформы и персонажа должна быть < 60 (не 50, а 60 на всякий случай)
                 * 
                 * 
                 * 
                 * Дальше проверяется пересечение координат по X
                 * 
                 * Если игрок хоть пикселем задевает платформу, то отталкивается от нее
                 * 
                */

                if((Math.Abs(y_OfPlatformLeft - y_OfuiLeft - 50) < 10) && (Math.Abs(y_OfPlatformLeft - y_OfuiLeft- 50) > 0))

                    if (((x_OfPlatformLeft <= x_OfuiLeft) && (x_OfuiLeft <= x_OfPlatformLeft + target.Width)) ||
                        ((x_OfPlatformLeft <= x_OfuiLeft + 50) && (x_OfuiLeft + 50 <= x_OfPlatformLeft + target.Width)))
                    {

                        return true;

                    }

            }

            return false;

        }

        //Метод смены позиции игрока на игровом поле. Вызывается каждую 1 мс
        public void ChangePlayerPosition()
        {

            //Всегда прыгает и проверяет на взаимодействие с платформой при падении
            Jump();

            //Если игрок отпустил клавишу, то начинаем сброс скорости до нуля
            if (resetVelocity) ResetVelocity();

            //Получаем X позиции игрока
            double xPos = (double)this.GetValue(Canvas.LeftProperty) + CurrentVelocity;

            //Получаем Y позиции игрока
            double yPos = (double)this.GetValue(Canvas.TopProperty) - CurrentJump;

            //Если игрок выходит за пределы игровой зоны, то его перемещает на противоположную сторону
            //как и в оригинале игры
            if (xPos < -35) { xPos = 565; }
            else if (xPos > 565) { xPos = -35; }

            //Устанавливаем позицию игрока по X
            this.SetValue(Canvas.LeftProperty, xPos);

            //Устанавливаем позицию игрока по Y
            this.SetValue(Canvas.TopProperty, yPos);

        }

    }

}
