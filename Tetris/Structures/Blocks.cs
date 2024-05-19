/*
 * Filename     : Block.cs
 * Purpose      : This class will be useful in the creation and rotation of block and also getting random blocks.
*/
using System;
using System.Collections.Generic;

namespace Tetris.Structures
{
    public class Blocks
    {
        //List to store all the blocks
        private List<int[,]> lstBlocks;

        //List of block 
        private List<IBlock> _blocks;
        public Blocks()
        {
            lstBlocks = new List<int[,]>();
            _blocks = new List<IBlock>();
            GenerateBlocks();
        }//ctor 

        private void GenerateBlocks()
        {
            //The basic Tetris blocks are defined below

            //Square Block
            int[,] block = new int[,] { {1,1 } , {1,1} };//2 rows and 2 columns
            IBlock _block = new Block(block, TypeOfBlock.Square_Block); 
            lstBlocks.Add(block);
            _blocks.Add(_block);

            //L1 Block
            block = new int[,] { { 2, 0 }, { 2, 0 }, { 2, 2 } };//3 rows and 2 columns
            _block = new Block(block, TypeOfBlock.L1_Block);
            lstBlocks.Add(block);
            _blocks.Add(_block);


            //L2 Block
            block = new int[,] { { 0, 3 }, { 0, 3 }, { 3, 3 } };//3 rows and 2 columns
            _block = new Block(block, TypeOfBlock.L2_Block);
            lstBlocks.Add(block);
            _blocks.Add(_block);


            //Z1 Block
            block = new int[,] { { 0, 4, 4 }, { 4, 4, 0 } };//2 rows and 3 columns
            _block = new Block(block, TypeOfBlock.Z1_Block);
            lstBlocks.Add(block);
            _blocks.Add(_block);



            //Z2 Block
            block = new int[,] { { 5, 5, 0 }, { 0, 5, 5 } };//2 rows and 3 columns
            _block = new Block(block, TypeOfBlock.Z2_Block);
            lstBlocks.Add(block);
            _blocks.Add(_block);



            //The pyramid block
            block = new int[,] { { 0, 6, 0 }, { 6, 6, 6 } };//2 rows and 3 columns
            _block = new Block(block, TypeOfBlock.Pyramid_Block);
            lstBlocks.Add(block);
            _blocks.Add(_block);



            //Line Block 
            block = new int[,] { { 7, 7, 7, 7 } };//1 row and 4 columns
            _block = new Block(block, TypeOfBlock.Line_Block);
            lstBlocks.Add(block);
            _blocks.Add(_block);
        }//GenerateBlock

        public int[,] GetRandomBlock()
        {
            //Get a random number for the index of a random block in the list
            Random rndNex = new Random();
            int iblock = rndNex.Next(0, lstBlocks.Count);
            return lstBlocks[iblock];
        }//GetRandomBlock
        public IBlock GetRandomBlock(string bb = "")
        {
            //Get a random number for the index of a random block in the list
            Random rndNex = new Random();
            int iblock = rndNex.Next(0, _blocks.Count);
            return _blocks[iblock];
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
