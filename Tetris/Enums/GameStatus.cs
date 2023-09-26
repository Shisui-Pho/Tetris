/*
 * Filename     : GameStatus.cs
 * Purpose      : An enum that will be used to keep track of the game status.(This is mostly determined in the LogicalGrid class)
*/
namespace Tetris.Enums
{
    public enum MovementStatus
    {
        GameOver,
        Newblock,
        CannotMoveRight,
        CannotMoveLeft,
        CannotMoveDown,
        CanMove,
        Default
    }//GameStatus
}//namespace
