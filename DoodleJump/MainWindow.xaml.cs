using System.Windows;
using System.Windows.Input;

namespace DoodleJump
{

    public partial class MainWindow : Window
    {
        
        public MainWindow() { InitializeComponent(); }
        
        private void _Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            Player player = new Player(Canvas_Launcher, 0.1, 0.1, new Location(35.0, 200.0));

        }
        
        private void _Button_Start_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
            Window_Game windowGame = new Window_Game();
            
            windowGame.Show();
            
            this.Close();

        }
        
        private void _Button_Exit_PreviewMouseDown(object sender, MouseButtonEventArgs e) { this.Close(); }

    }

}
