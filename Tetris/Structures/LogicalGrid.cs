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

        public int RowCount { get; private set; }

        public int ColCount { get; private set; }

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
            //Set the grid
            Grid = new int[grid.Rows, grid.Columns];

            //Set the properties
            StartColumnPosition = Grid.GetLength(1) / 2;

            //Set the block size
            BlockSize = grid.BlockSize;

            //Set the borad sizes
            ColCount = Grid.GetLength(1);
            RowCount = Grid.GetLength(0);
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
            //Test();
        }//InitializeGrid
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
        public GameStatus MoveBlock(int[,] blockToMove, Direction action, int StartRow, int StartColumn)
        {
            if (action == Direction.Rotated)
            {
                throw new ArgumentException("Cannot move block that is being rotated, call AddRotatedBlockFirst");
            }
            if (!CanMove(blockToMove, StartRow, StartColumn, action, out GameStatus status)) //Evaluate all possible movements
                return status;

            //At this point we are ready to move
            MoveBlock(blockToMove, StartRow, StartColumn, action);

            return GameStatus.CanMove;
        }//MoveBlock
        private void MoveBlock(int[,] block_to_move,int startRow, int StartCol, Direction dir)
        {
            int colIndex = block_to_move.GetLength(1) + StartCol;
            //Loop through the rowns of the block starting at the bottom row
            for (int currentRow = block_to_move.GetLength(0) + startRow; currentRow > startRow ; currentRow--)
            {
                //Loop through the columns of the block
                for(int currentCol = StartCol; currentCol < StartCol + block_to_move.GetLength(1); currentCol++)
                {
                    switch (dir)
                    {
                        case Direction.MoveLeft:
                            //Move the block left
                            if (Grid[currentRow - 1, currentCol - 1] != 0)
                                continue;
                            Grid[currentRow - 1, currentCol - 1] = Grid[currentRow - 1, currentCol];
                            Grid[currentRow - 1, currentCol] = 0;
                            break;
                        case Direction.MoveRight:
                            //Move the block right
                            if (Grid[currentRow - 1, colIndex] != 0)
                                continue;
                            Grid[currentRow - 1, colIndex] = Grid[currentRow - 1, colIndex - 1];
                            Grid[currentRow - 1, colIndex - 1] = 0;

                            colIndex--;
                            break;
                        case Direction.MoveDown:
                            //Move the block down
                            if (Grid[currentRow, currentCol] != 0)
                                continue;
                            Grid[currentRow, currentCol] = Grid[currentRow - 1, currentCol];
                            Grid[currentRow -1, currentCol] = 0; //Make the previous row empty
                            break;
                        default:
                            break;
                    }//end switch
                }//end column
                //Reset the column index
                colIndex = block_to_move.GetLength(1) + StartCol;
            }//end row
        }//Move
        private bool CanMoveDown2(int[,] blockToMove, int StartRow,int StartCol,out GameStatus status)
        {
            //Assume that we cannot move down. hance new block
            status = GameStatus.Newblock;
            //Moving down means that there must be no block beneath the current block
            //First check for bounds
            int nexBlockRow = StartRow + blockToMove.GetLength(0);
            if(nexBlockRow >= RowCount)
            {
                //status = GameStatus.Newblock;
                return false;
            }//end if we are at the bottom

            int countLen = 0;
            //For special cases of L block, and Z blocks, some other conditions must be considered as well
            for(int currentCol = StartCol; currentCol < StartCol + blockToMove.GetLength(1); currentCol++)
            {
                //Here if the block can be moved, we have some cases to consider
                //-Case 1: If there is nothing underneath the block
                //-Case 2: If there is another block underneath but leaves a space for the current block to fit in
                //-Case 3: If there is a block underneath and the current block has a space at that position

                if (Grid[nexBlockRow, currentCol] == 0|| (Grid[nexBlockRow, currentCol] != 0 && Grid[nexBlockRow - 1, currentCol] == 0))
                    countLen++;
            }//end for
            if (countLen != blockToMove.GetLength(1))
                return false;

            status = GameStatus.CanMove;
            return true;
        }//CanMoveDown2
        private bool CanMoveLeft(int[,] blockToMove, int StartRow, int StartCol, out GameStatus status)
        {
            status = GameStatus.CannotMoveLeft;

            //First check for the bounds
            if (StartCol <= 0)
                return false;
            int countLen = 0;

            //Now check for all block on the left of the current block
            for(int currentRow = StartRow; currentRow < StartRow + blockToMove.GetLength(0); currentRow++)
            {
                //Check if there are no blocks on the left of the current block
                if (Grid[currentRow, StartCol - 1] == 0 || (Grid[currentRow, StartCol - 1] != 0 && Grid[currentRow, StartCol] == 0))
                    countLen++;
            }//end for

            if (countLen != blockToMove.GetLength(0))
                return false;

            status = GameStatus.CanMove;
            return true;
        }//CanMoveLeft
        private bool CanMoveRight(int[,] blockToMove, int StartRow, int StartCol, out GameStatus status)
        {
            status = GameStatus.CannotMoveRight;
            int nextColumn = StartCol + blockToMove.GetLength(1);
            //Check for bounds
            if (nextColumn >= ColCount)
                return false;
            int countLen = 0;
            //Now check for all block on the right of the block
            for(int currentRow = StartRow; currentRow < StartRow + blockToMove.GetLength(0); currentRow++)
            {
                if (Grid[currentRow, nextColumn] == 0 || (Grid[currentRow, nextColumn] != 0 && Grid[currentRow, nextColumn - 1] == 0))
                    countLen++;
            }
            if (countLen != blockToMove.GetLength(0))
                return false;

            status = GameStatus.CanMove;
            return true;
        }//CanMoveRight
        private bool CanMove(int[,] blockToMove, int StartRow, int StartCol, Direction direction, out GameStatus status)
        {
            //Assume that it is game over
            status = GameStatus.GameOver;

            //First check if it is game over
            if (IsGameOver(StartRow, StartCol, blockToMove))
                return false;
            switch (direction)
            {
                case Direction.MoveLeft:
                    return CanMoveLeft(blockToMove, StartRow, StartCol, out status);
                case Direction.MoveRight:
                    return CanMoveRight(blockToMove, StartRow, StartCol, out status);
                case Direction.MoveDown:
                    return CanMoveDown2(blockToMove, StartRow, StartCol, out status);
            }
            return default;
        }//CanMove
        private bool IsGameOver(int StartRow,int StartCol, int[,] block)
        {
            //The default starting position are ROW[5] COLUMN[5]
            if (StartRow != StartRowPosition)//Not in the first row
                return false; //It is not game over yet
            for (int col = 0; col < block.GetLength(1); col++)
            {
                if (Grid[StartRow + block.GetLength(0), StartCol + col] != 0)
                    return true;
            }//end for columns
            return false;
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
            if (IsGameOver(iRowRef, iColRef, blockToInsert))
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
