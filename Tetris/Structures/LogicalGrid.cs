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
        public LogicalGrid(GameGrid grid)
        {
            Grid = new int[grid.Rows, grid.Columns];
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

            //For testing
            Grid[5, 2] = 1;
        }//InitializeGrid
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
        public bool MoveBlock(int[,] blockToMove, MoveAction action, int StartRow, int StartColumn, ref int iScore)
        {
            if (action == MoveAction.Rotated)
                throw new Exception("Cannot move block that is being rotated, call AddRotatedBlockFirst");
            if (!CanMove(blockToMove, StartRow, StartColumn, action)) //Evaluate all possible movements
                return false;

            //AT THIS POINT THE BLOCK IS GOOD TO GO
            int iToStartRow = StartRow;
            int iToStartColumn = StartColumn;
            
            for(int irow = 0; irow<blockToMove.GetLength(0); irow++)
            {
                for(int iCol = 0; iCol< blockToMove.GetLength(1); iCol++)
                {
                    //Set the values in the grid
                    //NOTE : To prevent errors when moving we first check if the block value is worth moving or not
                    if(blockToMove[irow,iCol] != 0)
                        Grid[iToStartRow, iToStartColumn] = blockToMove[irow, iCol];

                    //Clearing Entries for left and right
                    //if (Grid[StartRow + irow, StartColumn + blockToMove.GetLength(0)] == blockToMove[irow, iCol])
                    if (action == MoveAction.MoveLeft)
                            Grid[StartRow + irow, StartColumn + blockToMove.GetLength(0)] = 0;
                    if (action == MoveAction.MoveRight)
                        Grid[StartRow + irow, StartColumn - 1] = 0;
                    iToStartColumn++;
                }//end for {columns}
                //Reset the starting position of the column
                iToStartColumn = StartColumn;
                //Increase the starting position of the row
                iToStartRow++;
            }//end for {rows}
             //Clearing entries for top
            if (action == MoveAction.MoveDown)
            {
                for (int iCol = 0; iCol < blockToMove.GetLength(1); iCol++)
                {
                    
                }
            }
            EvaluateRows(ref iScore);
            return default;
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
        private bool CanMove(int[,] blockToMove, int StartRow, int StartCol, MoveAction direction)
        {
            int iReference;
            //bool canMove = false;
            if (direction == MoveAction.MoveDown)
            {
                //Check grid bounds first
                if ((StartRow + blockToMove.GetLength(0)) > Grid.GetLength(0))
                    return false;

                if (CanMoveDown(blockToMove, StartRow, StartCol))
                    return true;
                else
                    return false;
            }//end if  {ToDown movement}

            #region BugDetected
            if (direction == MoveAction.MoveLeft)
            {
                //Check grid bounds first
                if (StartCol == 0)
                    return false;

                //Check the left of the block
                iReference = StartRow;
                for(int iRow = 0; iRow < blockToMove.GetLength(0); iRow++)
                {
                    //BUG DETECTED
                    if(Grid[iReference, StartCol - 1] != 0)
                        return false;
                    iReference++;
                }//end for {iRow}
                //If all tests are passed
                return true;
            }//end if {ToLeft movement}
            if(direction == MoveAction.MoveRight)
            {
                //Check grid bounds first
                if (StartCol >= Grid.GetLength(1))
                    return false;

                //Check the right of the block
                iReference = StartRow;
                for(int iRow = 0; iRow < blockToMove.GetLength(0); iRow++)
                {
                    //BUG DETECTION
                    if (Grid[iReference, (StartCol + blockToMove.GetLength(1))] != 0)
                        return false;
                    iReference++;
                }//end for {iCol}

                //If all tests are passed
                return true;
            }//end if {ToRight movement}
            #endregion

            //If none of the direction
            return false;
        }//CanMove
        private bool CanMoveDown(int[,] block, int iStartRow, int iStartCol)
        {
            //There are two situation that need to be tested
            //-Both of them apply to L1 and L2 block 
            //-First one apply to all blocks exluding the square and line block

            //int iStartRow = 0, iStartCol = 0;
            //int[,] block = new int[7,7];

            //Default Check
            int emptyRow = 0;            
            int iCount = 0;
            int iReference = iStartCol;
            for (int r = 0; r < block.Length; r++)
            {
                if (Grid[(iStartRow + block.GetLength(0)), iReference] == 0)
                {
                    iCount++;
                    emptyRow = r;
                }
                iReference++;
            }
            if (iCount == block.GetLength(0))
                return true;//Can move down
            if (iCount == 0)
                return false; //Cannot move down


            //SPECIAL BLOCKS CHECK CHECKS

            //First Situation for all blocks
            //This are the L blocks
            if(iCount == 1 && block.GetLength(1) == 2)
            {
                if (DownCount(block, iStartRow, iStartCol) == 2)
                    return true;

                //TO DO  Situation 2 for L1 and L2 blocks
                //All this positions are ralative to the position of the block in the grid
                int indexRow1 = 0, indexRow2 = 0, indexCol1 = 0, indexCol2 = 0;
                if (emptyRow == 1)
                {
                    //Determining the indecies of the empty spaces for L2
                    //-Since the blocks shapes are fixed, determining the indecies is easier by moving relative to the empty cell
                    indexRow1 = iStartRow + block.GetLength(0) ; indexCol1 = iStartCol + block.GetLength(1) - 1;
                    indexRow2 = indexRow1 - 1; indexCol2 = indexCol1 - 1;
                }
                if(emptyRow == 0)
                {
                    // Determining the indecies of the empty spaces for L1
                    indexRow2 = iStartRow + block.GetLength(0) - 2; indexCol2 = iStartCol + block.GetLength(1) + 1;
                    indexRow1 = indexRow2 + 2; indexCol1 = indexCol2 -1;
                }
                if (Grid[indexRow1, indexCol1] == 0 && Grid[indexRow2, indexCol2] == 0)
                    return true;
            }//end count 1

            if (iCount == 2)
            {
                //TO DO Situation 1
                //Recheck
                if (DownCount(block,iStartRow,iStartCol) == 3)
                    return true;
            }//end cout 2
            return false;
        }//IsPerfectMatch
        private int DownCount(int[,] block, int iStartRow, int iStartCol)
        {
            int iCount = 0;
            for (int i = 0; i < block.GetLength(1); i++)
            {
                if (Grid[iStartRow + block.GetLength(0) - 1, iStartCol + i] == 0
                    || Grid[iStartRow + block.GetLength(0), iStartRow + i] == 0)
                    iCount++;
            }//end for
            return iCount;
        }//DownCount
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
    }//class
}//namesace
