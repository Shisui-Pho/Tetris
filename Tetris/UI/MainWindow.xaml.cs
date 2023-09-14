using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Tetris.Services;
using Tetris.Structures;
using Tetris.Enums;
using Tetris.Game;
namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }//ctor main

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //TODO : Rework the resize feature
        }//Window_SizeChanged

        private void btnRestart_MouseEnter(object sender, MouseEventArgs e)
        {
            btnRestart.Background = Brushes.Red;
        }//btnRestart_MouseEnter
        private void btnRestart_MouseLeave(object sender, MouseEventArgs e)
        {
        }//btnRestart_MouseLeave
        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            if (grdGameOver.Visibility == Visibility.Visible)
                grdGameOver.Visibility = Visibility.Hidden;
            else
                grdGameOver.Visibility = Visibility.Visible;
        }//btnRestart_Click

        private  void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GamePlay gm = new GamePlay(gameGrid);
            //Blocks bl = new Blocks();
            //int[,] block = bl.GetRandomBlock();
            //int[,] rotatedblock = bl.RotateBlock(block);

        }//Window_Loaded
    }//Class
}//namespace
