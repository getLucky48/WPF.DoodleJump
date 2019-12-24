using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DoodleJump
{

    public partial class Platform : UserControl
    {

        //Канвас, нужен для движущихся платформ, чтобы знать границы игрового поля
        private Canvas _Canvas;

        //Левая верхняя точка
        public Location LeftUpPoint;

        //Левая нижняя точка
        public Location LeftDownPoint;

        //Правая верхняя точка
        public Location RightUpPoint;

        //Правая нижняя точка
        public Location RightDownPoint;

        //Конструктор
        public Platform()
        {

            InitializeComponent();

            _Type = "Platform";

            _MovingLeft = false;

            _deltaVelocity = 1.0;

        }

        //Конструктор
        public Platform(string tType, Canvas tCanvas)
        {

            InitializeComponent();

            SetTypePlatform(tType);

            _MovingLeft = false;

            _deltaVelocity = 1.0;

            _Canvas = tCanvas;

        }

        //Тип платформы: обычная, сломанная, движущихся
        //Platform, PlatformBroken, PlatformMoving соответственно
        private string _Type;

        //False - если движение вправо / для движущихся платформ
        private bool _MovingLeft;

        //Скорость движения по горизонтали для движущихся платформ
        private double _deltaVelocity;

        //Таймер (нужен для движущихся платформ)
        private DispatcherTimer _Timer;

        //Задаем тип платформу
        public void SetTypePlatform(string tType)
        {
            
            //Если ничего, то платформа будет обычной
            if(_Type == null) { _Type = "Platform"; }

            //Меняем путь текущего спрайта на путь нового спрайта (спрайт == картинка)
            string NewImage = Image_Platform.Source.ToString().Replace(_Type, tType);

            //Пытаемся установить новую картинку. Используем try catch, т.к. возможно, что заданной платформы не существует
            try
            {

                Image_Platform.Source = new BitmapImage(new Uri(NewImage));

            }
            catch (IOException) { };

            //Задаем новый тип
            _Type = tType;

        }

        //Возвращает тип платформы
        public string GetTypePlatform()
        {

            return _Type;

        }

        //Метод устанавливает четыре точки - углы платформы
        public void SetLocation()
        {

            LeftUpPoint = Location.GetLocation(this);

            LeftDownPoint = new Location(LeftUpPoint.X, LeftUpPoint.Y + this.Height);

            RightUpPoint = new Location(LeftUpPoint.X + this.Width, LeftUpPoint.Y);

            RightDownPoint = new Location(LeftUpPoint.X + this.Width, LeftUpPoint.Y + this.Height);

        }

        //Метод вызывается при добавлении элемента на игровое поле
        private void _UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            //Задаем локацию платформы
            this.SetLocation();

            //Если платформа двигающаяся
            if (_Type.Contains("PlatformMoving"))
            {

                //Настраиваем таймер на 1 мс
                _Timer = new DispatcherTimer();

                _Timer.Tick += new EventHandler(_MovePlatform);

                //Устанавливаем интервал в 1 мс
                _Timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

                //Запускаем таймер
                _Timer.Start();

            }

        }

        //Метод вызывается каждые 10 мс. Двигает синие платформы
        private void _MovePlatform(object sender, EventArgs e)
        {

            //Двигаемся влево
            if (_MovingLeft)
            {

                //Если левая точка платформы дошла до края, то меняем направление движения
                if (LeftUpPoint.X < _Canvas.ActualWidth * 0.05)
                {

                    _MovingLeft = !_MovingLeft;

                }
                //Иначе просто двигаем платформу
                else
                {

                    this.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) - _deltaVelocity);

                }

            }
            //Двигаемся вправо
            else
            {

                //Если правая точка платформы дошла до края, то меняем направление движения
                if (RightUpPoint.X > _Canvas.ActualWidth * 0.95)
                {

                    _MovingLeft = !_MovingLeft;

                }
                //Иначе просто двигаем платформу
                else
                {

                    this.SetValue(Canvas.LeftProperty, (double)this.GetValue(Canvas.LeftProperty) + _deltaVelocity);

                }

            }

            //Задаем новую локацию для текущей платформы
            this.SetLocation();

        }
        
        public async Task BreakPlatformAsync()
        {

            if (_Type.Contains("PlatformBroken"))
            {

                //Меняем путь текущего спрайта на путь нового спрайта (спрайт == картинка)
                string NewImage = Image_Platform.Source.ToString().Replace(_Type, "PlatformBroken_2");

                Image_Platform.Source = new BitmapImage(new Uri(NewImage));

                this.Height = Image_Platform.Height;

                await Task.Delay(500);

                HideThis();

            }

        }

        private void HideThis()
        {

            this.Visibility = Visibility.Hidden;
        }

    }

}
