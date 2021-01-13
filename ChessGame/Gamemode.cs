namespace ChessGame
{
    public abstract class Gamemode
    {
        public string Name { get; protected set; }
        public abstract Chessboard GenerateBoard();
    }
}