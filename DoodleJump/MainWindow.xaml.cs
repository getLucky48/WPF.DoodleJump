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

    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Player player = new Player(Canvas_Launcher, 10.0, 12.5, new Location(35.0, 200.0));

        }

        private void Button_Start_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            Window_Game windowGame = new Window_Game();

            windowGame.Show();

            this.Close();

        }

        private void Button_Exit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            this.Close();

        }
    }

}
