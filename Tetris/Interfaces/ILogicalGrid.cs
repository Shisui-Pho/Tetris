using Tetris.Enums;
namespace Tetris.Interfaces
{
    public interface ILogicalGrid
    {
        int[,] Grid { get;}
        int BlockSize { get; }
        void ResetGrid();
        bool InsertBlock(int[,] blockToInsert);
        MovementStatus MoveBlock(int[,] blockToMove,Direction direction, int StartRow, int StartColumn);
        bool AddRotatedBlock(int[,] oldBlock, int[,] newBlock, int iStartRow, int iStartCol);
        void EvaluateRowsAndRemove(ref int iScore);
        //bool CanMove(int[,] blockToMove,int StartRow, int StartCol,MoveAction direction);
    }//interface
}//namespace
