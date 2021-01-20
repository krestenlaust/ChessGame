using ChessGame.Pieces;

namespace ChessGame.Gamemodes
{
    public class PawnTestChess : Gamemode
    {
        public override Chessboard GenerateBoard(Player playerWhite, Player playerBlack)
        {
            Chessboard board = new Chessboard(8, 8, this, playerWhite, playerBlack);

            board[new Coordinate(4, 0)] = new King { Color = TeamColor.White };
            board[new Coordinate(4, 7)] = new King { Color = TeamColor.Black };

            board[new Coordinate(0, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(1, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(2, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(3, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(4, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(5, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(6, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(7, 1)] = new Pawn { Color = TeamColor.White };

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
