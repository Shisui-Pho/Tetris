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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tetris.Structures;

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
            //var cl = new Color() { R = byte.Parse("166"), G = byte.Parse("247"), B = byte.Parse("187") };
            btnRestart.Background = Brushes.Red;//new SolidColorBrush(Color.FromArgb(0,166, 247, 187));
        }//btnRestart_MouseEnter
        private void btnRestart_MouseLeave(object sender, MouseEventArgs e)
        {
            //btnRestart.Background = Brushes.Transparent;
        }//btnRestart_MouseLeave
        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation widthAnimation = new DoubleAnimation();
            widthAnimation.From = 160;
            widthAnimation.To = this.Width - 30;
            widthAnimation.Duration = new TimeSpan(5);
            btnRestart.BeginAnimation(Button.WidthProperty, widthAnimation);
        }//btnRestart_Click

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GameGrid grid = new GameGrid();
            grid.GetGrid(ref gameGrid);
            //grdGameOver.Visibility = Visibility.Visible;
        }//Window_Loaded
    }//Class
}//namespace
