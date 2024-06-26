﻿/*
 * Filename     : MapLogicalGrid.cs
 * Purpose      : Create a graphic representation of the 2d array grid
*/
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tetris.Structures;
namespace Tetris.Services
{
    public class MapLogicalGrid
    {
        private static readonly SolidColorBrush strokrBrush = new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue)) { Opacity= 0.65 };
        //private static readonly Color BackGroundColor_ = (Color)ColorConverter.ConvertFromString("#000064");
        public static void MapGrid(Canvas gridCanvas, LogicalGrid grid)
        {
            gridCanvas.Children.Clear();//Remove all the children of the grid canvas
            //gridCanvas.Background = new SolidColorBrush(BackGroundColor_);
            //Initial X and Y 
            //--Represents the (0,0) location at the top coner
            int X_Coordinate = 0;
            int Y_Coordinate = 0;

            System.Drawing.Size size = new System.Drawing.Size() { Width = grid.BlockSize, Height = grid.BlockSize };
            for (int iRow = 0; iRow < grid.Grid.GetLength(0); iRow++)
            {
                for (int iColumn = 0; iColumn < grid.Grid.GetLength(1); iColumn++)
                {
                    //Get the required values 
                    SolidColorBrush brush = GetBrush(grid.Grid[iRow, iColumn]);
                    MapColours(gridCanvas, X_Coordinate, Y_Coordinate, brush, size);

                    X_Coordinate += grid.BlockSize; //Move to the right
                }//end Column loop
                X_Coordinate = 0;//Start from zero again
                Y_Coordinate += grid.BlockSize;//Move Down
            }//end Row loop
        }//Map
        private static SolidColorBrush GetBrush(int iBlockValue)
        {
            if (iBlockValue == 1 || iBlockValue == 4 || iBlockValue == 7)
                return new SolidColorBrush(Color.FromRgb(byte.MinValue, byte.MaxValue, byte.MinValue));//Green
            if (iBlockValue == 2 || iBlockValue == 5 || iBlockValue == 8)
                return new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MinValue, byte.MinValue));//Red
            if (iBlockValue == 3 || iBlockValue == 6 || iBlockValue == 9)
                return new SolidColorBrush(Color.FromRgb(byte.MinValue, byte.MinValue, byte.MaxValue));//Blue

            return new SolidColorBrush(Color.FromRgb(byte.MinValue, byte.MinValue, byte.MinValue));
        }//ChooseColor
        private static void MapColours(Canvas gr, int X_Coordinate, int Y_Coordinate, SolidColorBrush br,System.Drawing.Size size)
        {
            Rectangle _rc = new Rectangle();
            _rc.Width = size.Width;
            _rc.Height = size.Height;
            _rc.Stroke = strokrBrush;
            _rc.Fill = br;
            _rc.StrokeThickness = 1.3;
            gr.Children.Add(_rc);
            //Add Set the position
            Canvas.SetLeft(_rc, X_Coordinate);
            Canvas.SetTop(_rc, Y_Coordinate);
        }//MapColours
    }//class
}//namespace
