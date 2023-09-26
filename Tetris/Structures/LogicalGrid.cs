/*
 * Filename     : LogicalGrid.cs
 * Purpose      : This file containes the core of the game, all the "game" logic related to the grid and block movement is defined here 
*/
using System;
using Tetris.Enums;
using Tetris.Interfaces;
namespace Tetris.Structures
{
    //Grid Model
    public class LogicalGrid : ILogicalGrid
    {
        /// <summary>
        /// The Logical grid of the tetris game(2 dimension).
        /// </summary>
        public int[,] Grid { get; }
        /// <summary>
        /// The size of each block in the grid based on the width if the canvas.
        /// </summary>
        public int BlockSize { get; }
        /// <summary>
        /// The starting position of the row.
        /// </summary>
        public readonly int StartRowPosition = 0;
        /// <summary>
        /// The starting position of the column
        /// </summary>
        public readonly int StartColumnPosition;
        /// <summary>
        /// Creates a new instance of a logical grid.
        /// </summary>
        /// <param name="grid">The game grid object that will be used to scale the logical grid.</param>
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
            Test();
        }//InitializeGrid
        private void Test()
        {

            //Grid[10, 5] = 5;
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
        /// <summary>
        /// Resets the tetris grid by setting all block values to zero.
        /// </summary>
        public void ResetGrid()
        {
            InitializeGrid();
        }//ResetGrid
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blockToMove">The 2 dimension block to be moved.</param>
        /// <param name="action">The dirction of movement.</param>
        /// <param name="StartRow">The index of the starting row.</param>
        /// <param name="StartColumn">The index of the starting Column.</param>
        /// <param name="iScore">The score players score.</param>
        /// <returns>Returns a movementStatus to show if a block was moved or not.</returns>
        public MovementStatus MoveBlock(int[,] blockToMove, Direction action, int StartRow, int StartColumn)
        {
            if (action == Direction.Rotated)
            {
                throw new ArgumentException("Cannot move block that is being rotated, call AddRotatedBlockFirst");
            }
            if (!CanMove(blockToMove, StartRow, StartColumn, action, out MovementStatus status)) //Evaluate all possible movements
                return status;

            //AT THIS POINT THE BLOCK IS GOOD TO GO
            
            int NumberOfRowsOfTheBlock = blockToMove.GetLength(0); //Number of rows
            int NumberOfColumnsOfTheColumn = blockToMove.GetLength(1);//Number of columns
            
            if( action == Direction.MoveDown)
            {
                //Grid[noRows + StartRow, StartColumn] = 5;
                 for(int TheLastRow = NumberOfRowsOfTheBlock + StartRow; TheLastRow > StartRow; TheLastRow--)
                 {
                    for(int TheStartingColumn = StartColumn; TheStartingColumn < StartColumn + NumberOfColumnsOfTheColumn; TheStartingColumn++)
                    {
                        if (Grid[TheLastRow, TheStartingColumn] == 0 && Grid[TheLastRow - 1, TheStartingColumn] != 0)
                        {
                            Grid[TheLastRow, TheStartingColumn] = Grid[TheLastRow - 1, TheStartingColumn];
                            Grid[TheLastRow - 1, TheStartingColumn] = 0;
                        }//end first if
                    }//end for current column
                 }//end for last row
            }//moving down
            if (action == Direction.MoveLeft)
            {
                for(int currentRow = StartRow; currentRow < StartRow + NumberOfRowsOfTheBlock; currentRow++)
                {
                    for(int currentColumn = StartColumn - 1; currentColumn< StartColumn + NumberOfColumnsOfTheColumn - 1; currentColumn++)
                    {
                        if (Grid[currentRow, currentColumn] == 0)
                        {
                            Grid[currentRow, currentColumn] = Grid[currentRow, currentColumn + 1];
                            Grid[currentRow, currentColumn + 1] = 0;
                        }//end if
                    }//end for columns
                }//end for row
            }//moving left

            if(action == Direction.MoveRight)
            {
                for(int currrentRow = StartRow;currrentRow<StartRow + NumberOfRowsOfTheBlock; currrentRow++)
                {
                    for(int currentColumn= StartColumn + NumberOfColumnsOfTheColumn; currentColumn > StartColumn ; currentColumn--)
                    {
                        int i = currentColumn - 1;
                        Grid[currrentRow, currentColumn] = Grid[currrentRow, i];
                        Grid[currrentRow, i] = 0;
                    }//end for columns
                }//end for rows
            }//Moveright
           
            //EvaluateRowsAndRemove(ref iScore);
            return MovementStatus.CanMove;
        }//MoveBlock
        private bool CanMove(int[,] blockToMove, int StartRow, int StartCol, Direction direction, out MovementStatus status)
        {
            int iReference;
            int noRows = blockToMove.GetLength(0);
            int noColumns = blockToMove.GetLength(1);
            //bool canMove = false;
            if (direction == Direction.MoveDown)
            {
                //Check grid bounds first
                if ((StartRow + noRows) >= Grid.GetLength(0))
                {
                    status = MovementStatus.Newblock;
                    return false;
                }
                MovementStatus _status;
                if (CanMoveDown(blockToMove, StartRow, StartCol, out _status))
                {
                    status = _status;
                    return true;
                }
                status = _status; 
                return false;
            }//end if  {ToDown movement}

            #region BugDetected
            if (direction == Direction.MoveLeft)
            {
                //Check grid bounds first
                if (StartCol <= 0)
                {
                    status = MovementStatus.CannotMoveLeft;
                    return false;
                }                    
                //Check the left of the block
                iReference = StartRow;
                for(int iRow = 0; iRow < noRows; iRow++)
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
            if(direction == Direction.MoveRight)
            {
                //Check grid bounds first
                if (StartCol + noColumns >= Grid.GetLength(1))
                {
                    status = MovementStatus.CannotMoveRight;
                    return false;
                }

                //Check the right of the block
                iReference = StartRow;
                for(int iRow = 0; iRow < noRows; iRow++)
                {
                    //BUG DETECTION
                    if (Grid[iReference, StartCol + noColumns] != 0)
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

            //Local variable that will be needed
            int noRows = block.GetLength(0);
            int noColumns = block.GetLength(1);
            int iCount = 0;

            int lastRow = iStartRow + noRows;

            //Bug to be fixed
            //0 0 0 0 0 0 0 0 0 0
            //0 0 0 0 0 0 0 0 0 0
            //0 0 0 0 0 0 0 0 0 0
            //0 0 0 0 0 0 0 0 0 0
            //0 0 0 0 0 0 0 0 0 0
            //0 0 0 0 1 1 0 0 0 0
            //0 0 0 2 2 1 1 0 0 0
            //0 0 1 1 0 0 0 0 0 0
            //0 1 1 0 0 0 0 0 0 0
            //0 0 0 0 0 0 0 0 0 0
            //The last condition in the first if statement leaves room for a small bug which causes the S block to move down even though it is not supposed to
            for (int currentColumn = iStartCol; currentColumn < noColumns + iStartCol; currentColumn++)
            {
                if ((Grid[lastRow, currentColumn] != 0 && Grid[lastRow - 1, currentColumn] == 0)
                    || (Grid[lastRow, currentColumn] == 0 && Grid[lastRow - 1, currentColumn] != 0
                    || Grid[lastRow, currentColumn] == 0 && Grid[lastRow - 1, currentColumn] == 0)//BUG HERE
                    )
                {
                    iCount++;
                }//end if
                else if ((Grid[lastRow- 2, currentColumn] == 0 && Grid[lastRow - 1, currentColumn] != 0 && noRows == 3 && noColumns == 2) 
                        && !(block[noRows -1, 0] ==1 && block[noRows - 1,1] == 1  ))
                    iCount++;                                                                           
            }//end for loop

            //Moving down means that the block fits perfectly or there's nothing below it
            if(iCount == noColumns)
            {
                status = MovementStatus.CanMove;
                return true;
            }//if can move down
            status = MovementStatus.Newblock;
            return false;
        }//IsPerfectMatch
        private bool IsGameOver(int StartRow,int StartCol, int[,] block)
        {
            //The default starting position are ROW[5] COLUMN[5]
            if (StartRow != StartRowPosition)//Not in the first row
                return true; //It is not game over yet
            for (int col = 0; col < block.GetLength(1); col++)
            {
                if (Grid[StartRow + block.GetLength(0), StartCol + col] != 0)
                    return false;
            }//end for columns
            return true;
        }//CheckForGameOver
        /// <summary>
        /// Insert a block inside the grid starting from the initial posistions.
        /// </summary>
        /// <param name="blockToInsert">The 2 dimension block to insert.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Add the newly rotated block to the grid starting from the position of the old block.
        /// </summary>
        /// <param name="oldBlock">The old block that was rottated(To remove).</param>
        /// <param name="newBlock">The new block that needs to be added.</param>
        /// <param name="iStartRow">The starting row index of the old block.</param>
        /// <param name="iStartCol">The starting column index of the old block.</param>
        public bool AddRotatedBlock(int[,] oldBlock, int[,] newBlock, int iStartRow, int iStartCol)
        {
            //Before doing the rotation, we must first check weather the block can be rotated or not
            if (CannotRotate(oldBlock, iStartRow, iStartCol))
                return false;

            ReMoveOldBlock(oldBlock, iStartRow, iStartCol);
            for (int i = 0; i < newBlock.GetLength(0); i++)
            {
                for (int j = 0; j < newBlock.GetLength(1); j++)
                {
                    //Place Block in the grid
                    Grid[iStartRow + i, iStartCol + j] = newBlock[i, j];
                }//end for {j}
            }//end for {i}
            return true;
        }//AddRotatedBlock
        private bool CannotRotate(int[,] oldBlock, int iStartRow, int iStartCol)
        {
            //TO NOTE : rows == columns and columns == rows for the new block
            int rowLength = oldBlock.GetLength(0);
            int colLength = oldBlock.GetLength(1);

            //Checking with the assumption that it was rottated
            if (rowLength + iStartCol >= Grid.GetLength(1))
                return true;
            if (colLength + iStartCol >= Grid.GetLength(0))
                return true;

            //Check if it was urottated
            if (rowLength == colLength)
                return false;//This is a n x n block, there is no need for the rotation


            //Assuming that the block is roatated

            if (rowLength > colLength)
            {
                //This means that we have to check on the Right of the block
                //We just flip the block and check if it was rottated would there be obstecles that it will touch on the last column

                for (int i = 0; i < colLength; i++)
                {
                    if (Grid[iStartRow + i, iStartCol + rowLength - 1] != 0)
                        return true;
                }//end for columns
            }//end if rows are larger than the columns
            else
            {
                //This means that we have to check on the bottom of the block
                //We just flip the block and check if it was rottated would there be obstecles that it will touch at the last row
                for (int i = 0; i < rowLength; i++)
                {
                    if (Grid[iStartRow + colLength - 1, iStartCol + i] != 0)
                        return true;
                }//end for row
            }//end else (else the rows are less than the columns       
            return false;
        }//Can roate
        private void ReMoveOldBlock(int[,] block, int iStartRow, int iStartColum)
        {
            for (int i = 0; i < block.GetLength(0); i++)
            {
                for (int j = 0; j < block.GetLength(1); j++)
                {
                    //Replace the values of the block in the grid by zero {empty}
                    Grid[iStartRow + i, iStartColum + j] = 0;
                }//end for {j}
            }//end for {i}
        }//reMoveOldBlock
        public void EvaluateRowsAndRemove(ref int iScore)
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
    }//class
}//namesace
