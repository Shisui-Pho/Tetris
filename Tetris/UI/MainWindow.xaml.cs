/*
 * Filename     : MainWindow.xaml.cs
 * Purpose      : Will handle all the user interactions with the game
*/
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Tetris.Enums;
using Tetris.Services;
using Tetris.Structures;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Game objects
        private DispatcherTimer gameTimer;
        private readonly LogicalGrid L_GameGrid;
        //ReadOnly data fields
        private readonly GameGrid gridLayout;
        private readonly Blocks Blocks;

        //For game control
        private bool isGamePaused = false;
        //private static bool isMoveLeftPressed, isMoveRightPressed, isMoveDownPressed;
        private int CurrentScore = 0, HighScore = 0, StartRow, StartCol;
        private int[,] CurrentBlock;
        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            //Initialize readOnly Objects
            gridLayout = new GameGrid();
            gridLayout.GetGrid(gameGrid);
            //The Logical Grid
            L_GameGrid = new LogicalGrid(gridLayout);
            StartRow = L_GameGrid.StartRowPosition;
            StartCol = L_GameGrid.StartColumnPosition;
            Blocks = new Blocks();
        }//ctor main
        #region EventsHandlers
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            GameStatus st = L_GameGrid.MoveBlock(CurrentBlock, MoveDirection.MoveDown, StartRow, StartCol);
            if(st == GameStatus.CanMove)
            {
                StartRow++;
                MapLogicalGrid.MapGrid(gameGrid, L_GameGrid);
                lblCurrentScore.Content = CurrentScore;
            }//can move
            if (st == GameStatus.GameOver)
                GameOver();
            if (st == GameStatus.Newblock)
            {
                L_GameGrid.EvaluateRowsAndRemove(ref CurrentScore);
                NewBlock();
            }
        }//GameTimer_Tick
        private void CfrmTetrisGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (CurrentBlock == null)
                return;
            if (isGamePaused && !(e.Key == Key.P || e.Key == Key.Pause))
                return;
            GameStatus st = GameStatus.Default;
            if (e.Key == Key.Left || e.Key == Key.A)
            {
                st = L_GameGrid.MoveBlock(CurrentBlock, MoveDirection.MoveLeft, StartRow, StartCol);
                if (st == GameStatus.CanMove)
                {
                    StartCol--;
                    MapLogicalGrid.MapGrid(gameGrid, L_GameGrid);
                    lblCurrentScore.Content = CurrentScore;
                }//can move
                if (st == GameStatus.GameOver)
                    GameOver();
                //isMoveLeftPressed = true;
            }//Left key
            if( e.Key == Key.Right || e.Key == Key.D)
            {
                //Try to do the movement
                st = L_GameGrid.MoveBlock(CurrentBlock, MoveDirection.MoveRight, StartRow, StartCol);
                if (st == GameStatus.CanMove)
                {
                    StartCol++;
                    MapLogicalGrid.MapGrid(gameGrid, L_GameGrid);
                    lblCurrentScore.Content = CurrentScore;
                }//can move
                if (st == GameStatus.GameOver)
                    GameOver();
                //isMoveRightPressed = true;
            }//Right key
            if( e.Key == Key.Down || e.Key == Key.S || e.Key == Key.Space)
            {
                st = L_GameGrid.MoveBlock(CurrentBlock, MoveDirection.MoveDown, StartRow, StartCol);
                if(st == GameStatus.CanMove)
                {
                    StartRow++;
                    MapLogicalGrid.MapGrid(gameGrid, L_GameGrid);
                    lblCurrentScore.Content = CurrentScore;
                }//cann move
                if (st == GameStatus.GameOver)
                    GameOver();
                //isMoveDownPressed = true;
            }//Down key
            if(e.Key == Key.C || e.Key == Key.R)
            {
                int[,] rotatedblock = Blocks.RotateBlock(CurrentBlock);
                bool rotated = L_GameGrid.AddRotatedBlock(CurrentBlock, rotatedblock, StartRow, StartCol);
                if (rotated)
                {
                    CurrentBlock = rotatedblock;
                    MapLogicalGrid.MapGrid(gameGrid, L_GameGrid);
                }
            }//for block Rotation

            if(e.Key == Key.P || e.Key == Key.Pause)
            {
                if (!isGamePaused)
                {
                    grdPaused.Visibility = Visibility.Visible;
                    lblPausedScore.Text = "Score : " + CurrentScore;
                    gameTimer.Stop();
                    isGamePaused = true;
                }
                else
                    UnPauseGame();
            }//pause game

            //If a new block is needed
            if (st == GameStatus.Newblock)
            {
                L_GameGrid.EvaluateRowsAndRemove(ref CurrentScore);
                NewBlock();
            }//Create a new block
            //MessageBox.Show("key is now down");
        }//CfrmTetrisGame_KeyDown
        private void CfrmTetrisGame_KeyUp(object sender, KeyEventArgs e)
        {
        }//CfrmTetrisGame_KeyUp

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            UnPauseGame();
        }//btnContinue_Click

        private void CfrmTetrisGame_Loaded(object sender, RoutedEventArgs e)
        {
            btnRestart.Content = "Start Game";
        }//CfrmTetrisGame_Loaded
        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            btnRestart.Content = "Restart Game";
            RestartGame();
        }//btnRestart_Click
        #endregion


        #region Event Game Functions Helpers
        private void UnPauseGame()
        {
            grdPaused.Visibility = Visibility.Hidden;
            gameTimer.Start();
            isGamePaused = false;
        }//UnPauseGame
        private void InitializeGame()
        {
            //The game timer
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            gameTimer.Tick += GameTimer_Tick;
            //isMoveDownPressed = isMoveLeftPressed = isMoveRightPressed = false;
        }//InitializeGame
        private void NewBlock()
        {
            CurrentBlock = Blocks.GetRandomBlock();
            L_GameGrid.InsertBlock(CurrentBlock);
            StartRow = L_GameGrid.StartRowPosition;
            StartCol = L_GameGrid.StartColumnPosition;
            lblCurrentScore.Content = CurrentScore;
            MapLogicalGrid.MapGrid(gameGrid, L_GameGrid);
        }//NewBlock
        private void GameOver()
        {
            //gameTimer.Stop();
            MapLogicalGrid.MapGrid(gameGrid, L_GameGrid);
            grdGameOver.Visibility = Visibility.Visible;
            lblScore.Text = "Score : " + CurrentScore.ToString();
        }//GameOver
        private void RestartGame()
        {
            //GamePlay pl = new GamePlay(gameGrid);
            //Reset the starting and ending positions
            StartRow = L_GameGrid.StartRowPosition;
            StartCol = L_GameGrid.StartColumnPosition;

            //Change the scores
            if (CurrentScore > HighScore)
                HighScore = CurrentScore;
            CurrentScore = 0;

            //Reset the controls
            lblCurrentScore.Content = CurrentScore;
            lblHighScore.Content = HighScore;

            //Reset grid
            L_GameGrid.ResetGrid();
            CurrentBlock = Blocks.GetRandomBlock();
            L_GameGrid.InsertBlock(CurrentBlock);
            MapLogicalGrid.MapGrid(gameGrid, L_GameGrid);

            //isMoveDownPressed = isMoveLeftPressed = isMoveRightPressed = false;
            //Start the timer
            gameTimer.Start();
        }//RestartGame
        #endregion
    }//Class
    #region GamePlay Test
    //public class GamePlay
    //{
    //    private LogicalGrid Grid;
    //    private Blocks Blocks;
    //    private int[,] _ToMove;
    //    int i = 5, j = 5;
    //    private Canvas cn;
    //    public GamePlay(Canvas canvas)
    //    {
    //        GameGrid gridLayout = new GameGrid();
    //        gridLayout.GetGrid(canvas);
    //        Blocks = new Blocks();
    //        Grid = new LogicalGrid(gridLayout);
    //        _ToMove = Blocks.GetRandomBlock();
    //        Grid.AddRotatedBlock(_ToMove, _ToMove, i, j);
    //        MapLogicalGrid.MapGrid(canvas, Grid);
    //        DispatcherTimer tmr = new DispatcherTimer();
    //        tmr.Interval = new TimeSpan(0, 0, 1);
    //        tmr.Tick += Tmr_Tick;
    //        cn = canvas;
    //        tmr.Start();
    //    }//ctor 01
    //    private void Tmr_Tick(object sender, EventArgs e)
    //    {
    //        int iScore = 0;
    //        Grid.MoveBlock(_ToMove, Direction.MoveDown, i, j);
    //        MapLogicalGrid.MapGrid(cn, Grid);
    //        i++;
    //        //j++;
    //    }//Tmr_Tick
        #endregion
    //}//class
}//namespace
