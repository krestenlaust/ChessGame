using ChessGame.Pieces;

namespace ChessGame.Gamemodes
{
    public class PawnTestChess : Gamemode
    {
        public PawnTestChess(Player playerWhite, Player playerBlack) : base(playerWhite, playerBlack)
        {
        }

        public override Chessboard GenerateBoard()
        {
            Chessboard board = new Chessboard(8, 8, this);

            board[new Coordinate(4, 0)] = new King { Color = TeamColor.White };
            board[new Coordinate(4, 7)] = new King { Color = TeamColor.Black };

            board[new Coordinate(3, 3)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(4, 3)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(5, 3)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(6, 3)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(7, 3)] = new Pawn { Color = TeamColor.White };
            //board[new Coordinate(2, 3)] = new Hypnotist { Color = TeamColor.White };

            board[new Coordinate(0, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(1, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(2, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(3, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(4, 6)] = new Pawn { Color = TeamColor.Black };

            return board;
        }
    }
}
