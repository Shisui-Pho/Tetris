using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Structures
{
    public class Blocks
    {
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
            return default;
        }//GetRandomBlock
        public int[,] RotateBlock(int[,] blockTorotate)
        {
            return default;
        }//RotateBlock
    }//class
}//namespace
