using Tetris.Enums;
namespace Tetris.Interfaces
{
    public interface ILogicalGrid
    {
        int[,] Grid { get;}
        int BlockSize { get; }
        void ResetGrid();
        bool MoveBlock(int[,] blockToMove,MoveAction direction, int StartRow, int StartColumn, ref int iScore);
        void AddRotatedBlock(int[,] oldBlock, int[,] newBlock, int iStartRow, int iStartCol);
        //bool CanMove(int[,] blockToMove,int StartRow, int StartCol,MoveAction direction);
    }//interface
}//namespace
