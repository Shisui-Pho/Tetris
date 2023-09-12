using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Enums;
using Tetris.Services;
using Tetris.Structures;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Tetris.Game
{
    public class GamePlay
    {
        private LogicalGrid Grid;
        private Blocks Blocks;
        private int[,] _ToMove;
        int i = 5, j = 5;
        private Canvas cn;
        public GamePlay(Canvas canvas)
        {
            GameGrid gridLayout = new GameGrid();
            gridLayout.GetGrid(canvas);
            Blocks = new Blocks();
            Grid = new LogicalGrid(gridLayout);
            _ToMove = Blocks.GetRandomBlock();
            Grid.AddRotatedBlock(_ToMove, _ToMove,i,j);
            MapLogicalGrid.MapGrid(canvas, Grid);
            DispatcherTimer tmr = new DispatcherTimer();
            tmr.Interval = new TimeSpan(0, 0, 1);
            tmr.Tick += Tmr_Tick;
            cn = canvas;
            tmr.Start();
        }//ctor 01

        private void Tmr_Tick(object sender, EventArgs e)
        {
            int iScore = 0;
            Grid.MoveBlock(_ToMove, MoveAction.MoveDown, i, j, ref iScore);
            MapLogicalGrid.MapGrid(cn, Grid);
            i++;
            //j++;
        }//Tmr_Tick
    }//class
}//namespace
