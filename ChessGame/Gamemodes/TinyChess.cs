using ChessGame.Pieces;

namespace ChessGame.Gamemodes
{
    public class TinyChess : Gamemode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TinyChess"/> class.
        /// </summary>
        /// <param name="playerWhite"></param>
        /// <param name="playerBlack"></param>
        public TinyChess(Player playerWhite, Player playerBlack)
            : base(playerWhite, playerBlack)
        {
        }

        public override Chessboard GenerateBoard()
        {
            var board = new Chessboard(6, 8, this);

            board[new Coordinate(0, 0)] = new Rook(TeamColor.White);
            board[new Coordinate(1, 0)] = new Knight(TeamColor.White);
            board[new Coordinate(2, 0)] = new Bishop(TeamColor.White);
            board[new Coordinate(3, 0)] = new King(TeamColor.White);
            board[new Coordinate(4, 0)] = new Knight(TeamColor.White);
            board[new Coordinate(5, 0)] = new Rook(TeamColor.White);

            for (int file = 0; file < board.Width; file++)
            {
                board[new Coordinate(file, 1)] = new Pawn(TeamColor.White);
            }

            board[new Coordinate(0, 7)] = new Rook(TeamColor.Black);
            board[new Coordinate(1, 7)] = new Knight(TeamColor.Black);
            board[new Coordinate(2, 7)] = new Bishop(TeamColor.Black);
            board[new Coordinate(3, 7)] = new King(TeamColor.Black);
            board[new Coordinate(4, 7)] = new Knight(TeamColor.Black);
            board[new Coordinate(5, 7)] = new Rook(TeamColor.Black);

            for (int file = 0; file < board.Width; file++)
            {
                board[new Coordinate(file, 6)] = new Pawn(TeamColor.Black);
            }

            return board;
        }
    }
}
