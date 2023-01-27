using ChessGame.Pieces;

namespace ChessGame.Gamemodes
{
    public class CheckMateTest : Gamemode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckMateTest"/> class.
        /// </summary>
        /// <param name="playerWhite"></param>
        /// <param name="playerBlack"></param>
        public CheckMateTest(Player playerWhite, Player playerBlack)
            : base(playerWhite, playerBlack)
        {
        }

        public override Chessboard GenerateBoard()
        {
            var board = new Chessboard(8, 8, this);

            board[new Coordinate(0, 7-0)] = new King(TeamColor.White);
            board[new Coordinate(2, 2)] = new Pawn(TeamColor.White);
            board[new Coordinate(3, 2)] = new Pawn(TeamColor.White);
            board[new Coordinate(4, 7-0)] = new King(TeamColor.Black);
            board[new Coordinate(4, 5)] = new Pawn(TeamColor.Black);
            board[new Coordinate(5, 5)] = new Pawn(TeamColor.Black);
            /*
            board[new Coordinate(0, 0)] = new King(TeamColor.White);
            board[new Coordinate(1, 0)] = new Rook(TeamColor.White);
            board[new Coordinate(2, 0)] = new Rook(TeamColor.White);

            board[new Coordinate(0, 7)] = new King(TeamColor.Black);
            board[new Coordinate(1, 7)] = new Rook(TeamColor.Black);
            board[new Coordinate(2, 7)] = new Rook(TeamColor.Black);*/

            return board;
        }
    }
}
