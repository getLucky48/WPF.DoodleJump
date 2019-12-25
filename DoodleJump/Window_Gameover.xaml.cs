using System.Windows;
using System.Windows.Input;

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
            
            Window_Game gameWindow = new Window_Game();
            
            gameWindow.Show();
            
            this.Close();

        }
        
        private void Button_Menu_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
            MainWindow mainWindow = new MainWindow();
            
            mainWindow.Show();
            
            this.Close();

        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            Label_Score.Content = "Score: " + _Score.ToString();

        }

    }

}
