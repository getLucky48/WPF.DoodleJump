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
using System.Windows.Shapes;

namespace DoodleJump
{

    public partial class Window_Gameover : Window
    {

        private long _Score;

        public Window_Gameover()
        {

            _Score = 0;

            InitializeComponent();

        }

        public Window_Gameover(long tScore)
        {

            _Score = tScore;

            InitializeComponent();

        }

        private void Button_PlayAgain_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //Открыть игру
            Window_Game gameWindow = new Window_Game();

            gameWindow.Show();

            this.Close();

        }

        private void Button_Menu_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            //Открыть лаунчер
            Window_GameOver mainWindow = new Window_GameOver();

            mainWindow.Show();

            this.Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            Label_Score.Content = "Score: " + _Score.ToString();

        }

    }

}
