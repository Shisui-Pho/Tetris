namespace Tetris.Structures
{
    public class Block : IBlock
    {
        private int[,] _block;
        //Properties
        public int Length { get; private set; }
        public int Height { get; private set; }
        public TypeOfBlock TypeOfBlock { get; private set; }
        //Indexer
        public int this[int row, int col] => _block[row, col];
        public Block(int[,] block, TypeOfBlock type)
        {
            this._block = block;
            this.TypeOfBlock = type;

            //Get the size of the block
            this.Height = block.GetLength(0);
            this.Length = block.GetLength(1);
        }//class
        public void RotateCounterClockWise()
        {
            this._block = RotateCounterClockWise(_block);
        }//RotateCounterClockWise

        public int[,] GetMatrix()
        {
            return _block;
        }//GetBlock
        public static int[,] RotateCounterClockWise(int[,] blockToRotate)
        { 
            //The new block dimensions should be the dimensions of the block but swapped
            int[,] newBlock = new int[blockToRotate.GetLength(1), blockToRotate.GetLength(0)];

            //For resetting and increamenting the rows and columns indexes for the values
            int colIndex, rowIndex = 0;

            //Block Rotation
            for (int iCol = blockToRotate.GetLength(1) - 1; iCol >= 0; iCol--)
            {
                colIndex = 0; //Reset the starting index position to zero
                for (int iRow = 0; iRow < blockToRotate.GetLength(0); iRow++)
                {
                    //Retrieve values form the old block and insert them into the new block
                    newBlock[rowIndex, colIndex] = blockToRotate[iRow, iCol];
                    colIndex++;
                }//end for
                rowIndex++;
            }//end for
            return newBlock;
        }//RotateCountClockWise
    }//Block
}//namespace
