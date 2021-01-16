using ChessGame.Pieces;

namespace ChessGame.Gamemodes
{
    /// <summary>
    /// Chess with regular board size and no timer.
    /// </summary>
    public class TurtleChess : Gamemode
    {
        public TurtleChess(Player playerWhite, Player playerBlack) : base(playerWhite, playerBlack)
        {

        }

        protected override Chessboard GenerateBoard()
        {
            Chessboard board = new Chessboard(8, 8);

            board[new Coordinate(0, 0)] = new Rook { Color = TeamColor.White };
            board[new Coordinate(1, 0)] = new Knight { Color = TeamColor.White };
            board[new Coordinate(2, 0)] = new Bishop { Color = TeamColor.White };
            board[new Coordinate(3, 0)] = new Queen { Color = TeamColor.White };
            board[new Coordinate(4, 0)] = new King { Color = TeamColor.White };
            board[new Coordinate(5, 0)] = new Bishop { Color = TeamColor.White };
            board[new Coordinate(6, 0)] = new Knight { Color = TeamColor.White };
            board[new Coordinate(7, 0)] = new Rook { Color = TeamColor.White };

            board[new Coordinate(0, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(1, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(2, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(3, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(4, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(5, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(6, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(7, 1)] = new Pawn { Color = TeamColor.White };

            board[new Coordinate(0, 7)] = new Rook { Color = TeamColor.Black };
            board[new Coordinate(1, 7)] = new Knight { Color = TeamColor.Black };
            board[new Coordinate(2, 7)] = new Bishop { Color = TeamColor.Black };
            board[new Coordinate(3, 7)] = new Queen { Color = TeamColor.Black };
            board[new Coordinate(4, 7)] = new King { Color = TeamColor.Black };
            board[new Coordinate(5, 7)] = new Bishop { Color = TeamColor.Black };
            board[new Coordinate(6, 7)] = new Knight { Color = TeamColor.Black };
            board[new Coordinate(7, 7)] = new Rook { Color = TeamColor.Black };

            board[new Coordinate(0, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(1, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(2, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(3, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(4, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(5, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(6, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(7, 6)] = new Pawn { Color = TeamColor.Black };

            return board;
        }
    }
}
