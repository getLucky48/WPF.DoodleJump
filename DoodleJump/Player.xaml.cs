using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace DoodleJump
{

    public partial class Player : UserControl
    {

        //Максимальная высота прыжка
        private static double _MaxJump;

        //Максимальная скорость игрока по сторонам
        private double _MaxVelocity;

        //Текущая скорость игрока по сторонам. Нужно для плавного передвижения
        private double _CurrentVelocity;

        //Максимальная скорость прыжка
        private double _MaxVelocityJump;

        //Текущая скорость прыжка. Нужно для плавного подъема и спуска
        private double _CurrentVelocityJump;

        //Канвас - на чем рисуем и размещаем объекты. Необходимо для проверки координат платформ
        private Canvas _ParentCanvas;

        //Если true - значит, игрок падает, и мы ищем взаимодействие с платформой для следующего прыжка
        private bool _IsFalling;

        //Изменение скорости
        private double _deltaVelocity;

        //Изменение скорости прыжка
        private double _deltaVelocityJump;

        //Измеднение скорости при отпуске клавиши
        private double _deltaResetVelocity;

        //Если true - начинаем снижение скорости
        public bool resetVelocity;

        //Живой?
        public bool isAlive;

        //Счет
        public long score;

        //Возвращает максимальную высоту прыжка
        public static double GetMaxJump() { return _MaxJump; }

        //Конструктор класса. Создаем игрока, задаем канвас для размещения картинки, макс. скорость, макс. прыжок и локацию
        //Location - мой класс, хранит два double-значения X, Y и парочку методов
        public Player(Canvas tCanvas, double tVelocity, double tJump, Location tLocation)
        {


            _MaxVelocity = tVelocity;
            _MaxVelocityJump = tJump;

            _deltaVelocity = _MaxVelocity * 0.3;
            _deltaVelocityJump = _MaxVelocityJump * 0.08;
            _deltaResetVelocity = _MaxVelocity * 0.05;

            _CurrentVelocity = 0;
            score = 0;

            _ParentCanvas = tCanvas;

            //Добавляем на канвас нашего персонажа
            _ParentCanvas.Children.Insert(0, this);

            //SetLocation - мой метод. Описан ниже. Задает локацию для [игрока this]
            SetLocation(this, tLocation);

            _IsFalling = false;
            resetVelocity = false;
            isAlive = true;

            //Сымитируем прыжок игрока и высчитаем его максимальную высоту
            double temp_currentVelocityJump = 0;

            //Максимальная высота прыжка
            _MaxJump = 0;

            //Пока не достигнута максимальная скорость
            while(temp_currentVelocityJump < _MaxVelocityJump)
            {

                //Прибавляем скорость
                temp_currentVelocityJump += _deltaVelocityJump;

                //Прибавляем текущую скорость
                _MaxJump += temp_currentVelocityJump;

            }

            //Когда достигнут максимум скорости, то она начинает падать
            while(temp_currentVelocityJump > 0)
            {

                //Уменьшаем скорость
                temp_currentVelocityJump -= _deltaVelocityJump;

                //Прибавляем текущую скорость
                _MaxJump += temp_currentVelocityJump;

            }

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

            if (_CurrentVelocity - _deltaVelocity > -_MaxVelocity) { _CurrentVelocity -= _deltaVelocity; }

        }

        //Движение вправо. Аналогично движению влево
        public void MoveRight()
        {

            Image_Player.FlowDirection = FlowDirection.LeftToRight;

            if (_CurrentVelocity + _deltaVelocity < _MaxVelocity) { _CurrentVelocity += _deltaVelocity; }

        }
        
        // Метод прыжка. Персонаж должен все время прыгать, поэтому метод вызывается каждую 1 мс.
        public void Jump()
        {

            //Если игрок не падает
            if (!_IsFalling)
            {

                //Если текущая скорость прыжка меньше максимальной, то увеличиваем
                //Иначе, говорим, что игрок достиг максимальной скорости и начинает сбавлять ее, а затем падать
                if (_CurrentVelocityJump + _deltaVelocityJump < _MaxVelocityJump) { _CurrentVelocityJump += _deltaVelocityJump; }
                else { _IsFalling = !_IsFalling; }

            }
            //Если игрок падает, то он уменьшает скорость прыжка
            else {

                //Уменьшаем скорость прыжка. Если < 0, значит он уже падает, а не сбавляет скорость
                _CurrentVelocityJump -= _deltaVelocityJump;

                //OnCollisionEnter - мой метод. Проверяет соприкасается ли объект с платформами.
                //true - если упал на платформу
                //Если игрок падает и касается платформы, то нужно от нее оттолкнуться
                if ((_CurrentVelocityJump < 0) && OnCollisionEnter(this))
                {

                    //Ставит скорость в ноль, т.к. было соприкосновение с платформой
                    _CurrentVelocityJump = 0.0;

                    //Если упал на платформу, значит уже не падает
                    _IsFalling = !_IsFalling;

                }

            }

        }

        //При отпуске клавиш управления A и D скорость уменьшается до нуля постепенно
        public void ResetVelocity()
        {

            //Если движение влево
            if (_CurrentVelocity < 0)
            {

                //По умолчанию я уменьшаю на 0.25. Вдруг, текущая скорость 0.1, то мы в ноль не попадем, поэтому такое ветвление
                if (_CurrentVelocity > -_deltaResetVelocity) { _CurrentVelocity = 0; }

                else { _CurrentVelocity += _deltaResetVelocity; }

            }

            //Если движение вправо
            else if (_CurrentVelocity > 0)
            {

                //По умолчанию я уменьшаю на 0.25. Вдруг, текущая скорость 0.1, то мы в ноль не попадем, поэтому такое ветвление
                if (_CurrentVelocity < _deltaResetVelocity) { _CurrentVelocity = 0; }

                else { _CurrentVelocity -= _deltaResetVelocity; }

            }

        }

        //Метод проверки столкновений. ui проверяемый элемент. В данном проекте - игрок.
        public bool OnCollisionEnter(UIElement ui)
        {

            //Получаем список всех дочерних элементов канваса - платформы
            //Затем перебираем в foreach
            for(int i = 0; i < _ParentCanvas.Children.OfType<Platform>().ToArray().Length; i++)
            {

                var target = _ParentCanvas.Children.OfType<Platform>().ElementAt(i);

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

                if((Math.Abs(y_OfPlatformLeft - y_OfuiLeft - this.Height) < target.Height) && (Math.Abs(y_OfPlatformLeft - y_OfuiLeft - this.Height) > 0))

                    if (((x_OfPlatformLeft <= x_OfuiLeft) && (x_OfuiLeft <= x_OfPlatformLeft + target.Width)) ||
                        ((x_OfPlatformLeft <= x_OfuiLeft + this.Width) && (x_OfuiLeft + this.Width <= x_OfPlatformLeft + target.Width)))
                    {

                        if (target.GetTypePlatform().Contains("PlatformBroken"))
                        {

                            target.BreakPlatformAsync();

                            return false;

                        }

                        return true;

                    }

            }

            return false;

        }

        //Метод смены позиции игрока на игровом поле. Вызывается каждую 1 мс
        public void ChangePlayerPosition()
        {

            //Если игрок упал ниже окна на 10% от высоты игрового поля, то игра окончена
            if(Location.GetLocation(this).Y > (_ParentCanvas.ActualHeight * 1.1))
            {

                isAlive = false;

                return;

            }

            //Всегда прыгает и проверяет на взаимодействие с платформой при падении
            Jump();

            //Если игрок отпустил клавишу, то начинаем сброс скорости до нуля
            if (resetVelocity) { ResetVelocity(); }

            //Получаем X позиции игрока
            double xPos = (double)this.GetValue(Canvas.LeftProperty) + _CurrentVelocity;

            //Получаем Y позиции игрока
            double yPos = (double)this.GetValue(Canvas.TopProperty) - _CurrentVelocityJump;

            //Если игрок выходит за пределы игровой зоны, то его перемещает на противоположную сторону
            //как и в оригинале игры
            if (xPos < -(_ParentCanvas.ActualWidth * 0.05) ) { xPos = _ParentCanvas.ActualWidth * 0.95; }
            else if (xPos > _ParentCanvas.ActualWidth * 0.95) { xPos = -(_ParentCanvas.ActualWidth * 0.05); }

            //Устанавливаем позицию игрока по X
            this.SetValue(Canvas.LeftProperty, xPos);

            //Устанавливаем позицию игрока по Y
            this.SetValue(Canvas.TopProperty, yPos);

        }

    }

}
