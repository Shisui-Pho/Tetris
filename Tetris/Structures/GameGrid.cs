/*
 * Filename     :GameGrid.cs
 * Purpose      : Ceate the gridlines as well as the virtual grid properties
*/
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
namespace Tetris.Structures
{
    public class GameGrid
    {

        //Imporatant properties of the grid
        public int BlockSize { get; private set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public GameGrid(int BlockSize = 30)
        {
            this.BlockSize = BlockSize;
        }//ctor 01
        public void GetGrid(Canvas gridCanvas)
        {
            GenerateGrid(gridCanvas);
        }//GetGrid method

        private void GenerateGrid(Canvas gridCanvas)
        {
            List<Point[]> horizontalPoints = new List<Point[]>();
            List<Point[]> verticalPoints = new List<Point[]>();

            //For the vertical points
            //- End point 1 is (width,0)
            //-The start point has to be (0,0)
            int xEnd = (int)gridCanvas.Width;
            int yEnd = 0;

            //Set initial Points
            Point startPoint = new Point(0, 0);
            Point endPoint = new Point( xEnd, yEnd);

            //Number of lines
            Columns = (int)(gridCanvas.Width - 1) / BlockSize;
            Rows = (int)(gridCanvas.Height - 1) / BlockSize;

            //Set the horizontal poits for the grid
            for (int i= 0; i<= Rows; i++)
            {
               horizontalPoints.Add(new Point[] { startPoint, endPoint });
                //Only the Y values need to be increased
               startPoint.Y += BlockSize;
               endPoint.Y += BlockSize;
            }//end for loop

            //For the vertical points
            //- End point 1 is (0,height)
            //-the start point has to be (0,0)
            xEnd = 0;
            yEnd = (int)gridCanvas.Height;

            //Set initial Points
            startPoint = new Point(0, 0);
            endPoint = new Point(xEnd, yEnd);

            //Set the vertical Points
            for (int j=0; j<=Columns; j++)
            {
                verticalPoints.Add(new Point[] { startPoint, endPoint });
                //Only the x values need to be increased
                startPoint.X += BlockSize;
                endPoint.X += BlockSize;
            }//end for loop
            PlotPoints(ref gridCanvas, horizontalPoints, verticalPoints);
        }//CreateGridLayout

        private void PlotPoints(ref Canvas gridCanvas, List<Point[]> hPoints, List<Point[]> vPoints)
        {
            //Create new brush once
            var brush = new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color()
            {
                //The lines must be white
                A = byte.MaxValue,
                R = byte.MaxValue,
                B = byte.MaxValue,
                G = byte.MaxValue
            });
            brush.Opacity = 0.65;


            //First the vertical points
            foreach (Point[] p in vPoints)
            {
                gridCanvas.Children.Add(GetLine(p, brush));
            }//Draw vertical lines

            //Then the horizontal points
            foreach (Point[] p in hPoints)
            {
                gridCanvas.Children.Add(GetLine(p, brush));
            }//Draw horizontal lines
        }//PlotPoints
        private Line GetLine(Point[] p, System.Windows.Media.SolidColorBrush brush)
        {
            //Create the lines 
            Line ln = new Line();
            ln.StrokeThickness = 2;
            ln.Stroke = brush;
            ln.X1 = p[0].X;
            ln.X2 = p[1].X;
            ln.Y1 = p[0].Y;
            ln.Y2 = p[1].Y;
            return ln;
        }//GetLine
    }//class
}//namespace
