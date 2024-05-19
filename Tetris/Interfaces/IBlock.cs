namespace Tetris.Structures
{
    public interface IBlock
    {
        int Length { get; }
        int Height { get; }
        int this[int row, int col] { get; }
        TypeOfBlock TypeOfBlock { get; }
        void RotateCounterClockWise();
        int[,] GetMatrix();
    }//IBlock
}//namespace
