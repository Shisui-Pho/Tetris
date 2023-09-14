using System;
using Tetris.Enums;
using Tetris.Interfaces;
namespace Tetris.Structures
{
    //Grid Model
    public class LogicalGrid : ILogicalGrid
    {
        public int[,] Grid { get; }
        public int BlockSize { get; }
        public readonly int StartRowPosition = 1;
        public readonly int StartColumnPosition;
        public LogicalGrid(GameGrid grid)
        {
            Grid = new int[grid.Rows, grid.Columns];
            StartColumnPosition = Grid.GetLength(1) / 2;
            BlockSize = grid.BlockSize;
            InitializeGrid();
        }//LogicalGrid
        private void InitializeGrid()
        {
            //Set all the blocks to zero for the initial creation of the LogicalGrid
            for(int i = 0; i< Grid.GetLength(0); i++)
            {
                for (int j = 0; j < Grid.GetLength(1); j++)
                {
                    Grid[i, j] = 0;
                }//end for {j}
            }//end for {i}

        }//InitializeGrid
        private void Test()
        {
            //TESTING
            //Right Wall
            //for (int i = 0; i < Grid.GetLength(0); i++)
            //{
            //    Grid[i, Grid.GetLength(1) - 1] = 5;
            //}
            //For testing

            //LEFT WALL
            //for (int i = 0; i < Grid.GetLength(0); i++)
            //{
            //    Grid[i, 0] = 5;
            //}

            //BOTTOM WALL
            //for (int i = 0; i < Grid.GetLength(1); i++)
            //{
            //    Grid[Grid.GetLength(0) - 1, 0] = 5;
            //}

            //Obsticle
            //Grid[10, 5] = 1;
        }
        public void ResetGrid()
        {
            //Re-initialize the grid
            InitializeGrid();
        }//ResetGrid
        /// <summary>
        /// PLEASE INCREMENT AFTER THE MOVEMENT WAS MADE
        /// </summary>
        /// <param name="blockToMove">The 2d array block</param>
        /// <param name="action">The action to be perfomed</param>
        /// <param name="StartRow">The starting row before increment</param>
        /// <param name="StartColumn">The Starting Column before the increament</param>
        /// <param name="iScore">The reference object for the score</param>
        /// <returns>Indicates if the block was able to be moved.</returns>
        public MovementStatus MoveBlock(int[,] blockToMove, MoveAction action, int StartRow, int StartColumn, ref int iScore)
        {
            if (action == MoveAction.Rotated)
            {
                throw new ArgumentException("Cannot move block that is being rotated, call AddRotatedBlockFirst");
            }
            if (!CanMove(blockToMove, StartRow, StartColumn, action, out MovementStatus status)) //Evaluate all possible movements
                return status;

            //AT THIS POINT THE BLOCK IS GOOD TO GO
            //int iToStartRow = StartRow;
            //int iToStartColumn = StartColumn;
            
            int noRows = blockToMove.GetLength(0); //Number of rows
            int noColumns = blockToMove.GetLength(1);//Number of columns
            
            //If the move action is downwards
            if (action == MoveAction.MoveDown)
            {
                for(int iCurrentRow = StartRow + noRows ; iCurrentRow >= StartRow; iCurrentRow--)
                {
                    for (int iCurrentColumn = StartColumn; iCurrentColumn < StartColumn + noColumns; iCurrentColumn++)
                    {
                        if (Grid[iCurrentRow - 1, iCurrentColumn] != 0 
                            ||(Grid[iCurrentRow, iCurrentColumn] == Grid[iCurrentRow+1, iCurrentColumn]))
                        {
                            Grid[iCurrentRow, iCurrentColumn] = Grid[iCurrentRow - 1, iCurrentColumn];
                        }
                    }//end columns loop
                }//end rows loop
            }//end moving down movement
            if (action == MoveAction.MoveLeft)
            {
                for (int iCurrentColumn = StartColumn -1; iCurrentColumn < StartColumn + noColumns + 1; iCurrentColumn++)
                {
                    //TO DO : Move the block left
                    for (int iCurrentRow = StartRow; iCurrentRow < noRows + StartRow; iCurrentRow++)
                    {
                      
                            if (Grid[iCurrentRow, iCurrentColumn + 1] >= 0)
                                Grid[iCurrentRow, iCurrentColumn] = Grid[iCurrentRow, iCurrentColumn + 1];
                    
                    }//end for
                }//end for
            }//end moving down movement

            if (action == MoveAction.MoveRight)
            {
                for (int iCurrentColumn = StartColumn + noColumns; iCurrentColumn >= StartColumn; iCurrentColumn--)
                {
                    for (int iCurrentRow = StartRow; iCurrentRow < noRows + StartRow; iCurrentRow++)
                    {
                        if (Grid[iCurrentRow, iCurrentColumn - 1] >= 0)
                            Grid[iCurrentRow, iCurrentColumn] = Grid[iCurrentRow, iCurrentColumn - 1];
                    }
                }//end for
            }//end moving down movement
            EvaluateRows(ref iScore);
            return MovementStatus.CanMove;
        }//MoveBlock
        public void AddRotatedBlock(int[,] oldBlock, int[,] newBlock, int iStartRow, int iStartCol)
        {
            ReMoveOldBlock(oldBlock, iStartRow, iStartCol);
            for (int i = 0; i < newBlock.GetLength(0); i++)
            {
                for (int j = 0; j < newBlock.GetLength(1); j++)
                {
                    //Place Block in the grid
                    Grid[iStartRow + i, iStartCol + j] = newBlock[i, j];
                }//end for {j}
            }//end for {i}
        }//AddRotatedBlock
        private void ReMoveOldBlock(int[,] block, int iStartRow, int iStartColum)
        {
            for(int i = 0; i< block.GetLength(0); i++)
            {
                for (int j = 0; j < block.GetLength(1); j++)
                {
                    //Replace the values of the block in the grid by zero {empty}
                    Grid[iStartRow + i, iStartColum + j] = 0;
                }//end for {j}
            }//end for {i}
        }//reMoveOldBlock
        private bool CanMove(int[,] blockToMove, int StartRow, int StartCol, MoveAction direction, out MovementStatus status)
        {
            int iReference;
            //bool canMove = false;
            if (direction == MoveAction.MoveDown)
            {
                //Check grid bounds first
                if ((StartRow + blockToMove.GetLength(0)) >= Grid.GetLength(0))
                {
                    status = MovementStatus.CannotMoveDown;
                    return false;
                }
                MovementStatus st;
                if (CanMoveDown(blockToMove, StartRow, StartCol, out st))
                {
                    status = st;
                    return true;
                }
                status = st; 
                return false;
            }//end if  {ToDown movement}

            #region BugDetected
            if (direction == MoveAction.MoveLeft)
            {
                //Check grid bounds first
                if (StartCol <= 0)
                {
                    status = MovementStatus.CannotMoveLeft;
                    return false;
                }                    
                //Check the left of the block
                iReference = StartRow;
                for(int iRow = 0; iRow < blockToMove.GetLength(0); iRow++)
                {
                    //BUG DETECTED
                    if(Grid[iReference, StartCol - 1] != 0)
                    {
                        status = MovementStatus.CannotMoveLeft;
                        return false;
                    }
                    iReference++;
                }//end for {iRow}
                //If all tests are passed
                status = MovementStatus.CanMove;
                return true;
            }//end if {ToLeft movement}
            if(direction == MoveAction.MoveRight)
            {
                //Check grid bounds first
                if (StartCol + blockToMove.GetLength(1) >= Grid.GetLength(1))
                {
                    status = MovementStatus.CannotMoveRight;
                    return false;
                }

                //Check the right of the block
                iReference = StartRow;
                for(int iRow = 0; iRow < blockToMove.GetLength(0); iRow++)
                {
                    //BUG DETECTION
                    if (Grid[iReference, StartCol + blockToMove.GetLength(1)] != 0)
                    {
                        status = MovementStatus.CannotMoveRight;
                        return false;
                    }
                    iReference++;
                }//end for {iCol}

                //If all tests are passed
                status = MovementStatus.CanMove;
                return true;
            }//end if {ToRight movement}
            #endregion

            //If none of the direction
            status = MovementStatus.Default;
            return false;
        }//CanMove
        private bool CanMoveDown(int[,] block, int iStartRow, int iStartCol, out MovementStatus status)
        {
            //Game over check should be first
            if(!IsGameOver(iStartRow, iStartCol, block))
            {
                status = MovementStatus.GameOver;
                return false;
            }//if game over

            int noRows = block.GetLength(0);
            int noColumns = block.GetLength(1);

            int iCount = 0;
            //Default Check  
            for (int Columns = 0; Columns < noColumns; Columns++)
            {       //1. The row has totally nothing underneath it......
                if (Grid[noRows + iStartRow, iStartCol + Columns] == 0
                    //The row makes a perfect fit with the block below it
                    || (Grid[noRows + iStartRow - 1, iStartCol + Columns] == 0 && Grid[noRows + iStartRow, iStartCol + Columns] != 0))
                {
                    iCount++;
                }//end if
                //iReference++;
            }//end for
            if (iCount == block.GetLength(1))
                //Can move down{
            {
                status = MovementStatus.CanMove;
                return true;
            }
            status = MovementStatus.Newblock;
            return false;
        }//IsPerfectMatch
        private bool IsGameOver(int StartRow,int StartCol, int[,] block)
        {
            //The default starting position are ROW[5] COLUMN[5]
            if (StartRow != StartRowPosition && StartCol != StartColumnPosition)//Not in the first row
                return true; //It is not game over yet
            for (int col = 0; col < block.GetLength(1); col++)
            {
                if (Grid[StartRow + block.GetLength(0), StartCol + col] != 0)
                    return false;
            }//
            return true;
        }//CheckForGameOver
        private void EvaluateRows(ref int iScore)
        {
            //Counters for loops
            int i, j;
            //Determinants
            int iCountBlockNumbers = 0;
            int iScore_Obtained = 0;

            //-Evaluation must start from the bottom row and move upwards
            for (i = Grid.GetLength(0) - 1; i >= 0; i--)
            {
                for (j = 0; j < Grid.GetLength(1); j++)
                {
                    if (Grid[i, j] != 0)
                    {
                        iCountBlockNumbers++;
                        iScore_Obtained += Grid[i, j];
                    }//Check the values in the grid
                }//Loop for columns

                //If there is a space "0" between a block in a single row, then the row must not be removed
                if (iCountBlockNumbers >= Grid.GetLength(1))
                {
                    iScore += iScore_Obtained;
                    ReplaceRows(i, Grid.GetLength(1));
                    i++;
                }//Check if there's no space
                iScore_Obtained = 0;
                iCountBlockNumbers = 0;
            }//Rows loop
            //GameOver();
        }//EvaluateRows
        private void ReplaceRows(int iStartRow, int iColumns)
        {
            //Row must be replace with all the rows above it
            //Loop counters
            int i, j;
            //Same as before, start from the bottom to the top
            for (i = iStartRow; i >= 0; i--)
            {
                for (j = 0; j < iColumns; j++)
                {
                    //Take the value above and put it in the current row
                    if (i != 0)
                        Grid[i, j] = Grid[i - 1, j];
                }//Loop throgh the columns
            }//Loop through the rows, starting from the bottom
        }//ReplaceRows

        public bool InsertBlock(int[,] blockToInsert)
        {
            int iColRef = StartColumnPosition;
            int iRowRef = StartRowPosition;
            if (!IsGameOver(iRowRef, iColRef, blockToInsert))
                return false;
            for (int row = 0; row < blockToInsert.GetLength(0); row++)
            {
                for (int col = 0; col < blockToInsert.GetLength(1); col++)
                {
                    Grid[iRowRef, iColRef] = blockToInsert[row, col];
                    iColRef++;
                }//end col for
                iColRef = StartColumnPosition;
                iRowRef++;
            }//end row for
            return true;
        }//InsertBlock
    }//class
}//namesace
