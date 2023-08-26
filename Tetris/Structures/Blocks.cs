using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Structures
{
    public class Blocks
    {
        //List to store all the blocks
        private List<int[,]> lstBlocks;
        public Blocks()
        {
            lstBlocks = new List<int[,]>();
            GenerateBlocks();
        }//ctor 

        private void GenerateBlocks()
        {
            //The basic Tetris blocks are defined below

            //Square Block
            int[,] block = new int[,] { {1,1 } , {1,1} };//2 rows and 2 columns
            lstBlocks.Add(block);

            //L1 Block
            block = new int[,] { { 2, 0 }, { 2, 0 }, { 2, 2 } };//3 rows and 2 columns
            lstBlocks.Add(block);

            //L2 Block
            block = new int[,] { { 0, 3 }, { 0, 3 }, { 3, 3 } };//3 rows and 2 columns
            lstBlocks.Add(block);

            //Z1 Block
            block = new int[,] { { 0, 4, 4 }, { 4, 4, 0 } };//2 rows and 3 columns
            lstBlocks.Add(block);

            //Z2 Block
            block = new int[,] { { 5, 5, 0 }, { 0, 5, 5 } };//2 rows and 3 columns
            lstBlocks.Add(block);

            //The pyramid block
            block = new int[,] { { 0, 6, 0 }, { 6, 6, 6 } };//2 rows and 3 columns
            lstBlocks.Add(block);

            //Line Block 
            block = new int[,] { { 7, 7, 7, 7 } };//1 row and 4 columns
            lstBlocks.Add(block);
        }//GenerateBlock

        public int[,] GetRandomBlock()
        {
            //Get a random number for the index of a random block in the list
            Random rndNex = new Random();
            int iblock = rndNex.Next(0, lstBlocks.Count);
            return lstBlocks[iblock];
        }//GetRandomBlock
        public int[,] RotateBlock(int[,] blockToRotate)
        {
            //The new block dimensions should be the dimensions of the block but swapped
            int[,] newBlock = new int[blockToRotate.GetLength(1), blockToRotate.GetLength(0)];

            //For resetting and increamenting the rows and columns indexes for the values
            int colIndex, rowIndex = 0;

            //Block Rotation
            for(int iCol = blockToRotate.GetLength(1) - 1; iCol >= 0; iCol--)
            {
                colIndex = 0; //Reset the starting index position to zero
                for(int iRow = 0; iRow < blockToRotate.GetLength(0); iRow++)
                {
                    //Retrieve values form the old block and insert them into the new block
                    newBlock[rowIndex, colIndex] = blockToRotate[iRow, iCol];
                    colIndex++;
                }//end for
                rowIndex++;
            }//end for
            return newBlock;
        }//RotateBlock
    }//class
}//namespace
