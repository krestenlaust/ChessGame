namespace ChessGame.Gamemodes
{
    public class OpenWorldChess : Gamemode
    {
        public override Chessboard GenerateBoard(Player playerWhite, Player playerBlack)
        {
            Chessboard board = new Chessboard(14, 20, this, playerWhite, playerBlack);

            board[new Coordinate(0, 0)] = new Pieces.Rook { Color = TeamColor.White };
            board[new Coordinate(1, 0)] = new Pieces.Knight { Color = TeamColor.White };
            board[new Coordinate(2, 0)] = new Pieces.Bishop { Color = TeamColor.White };
            board[new Coordinate(3, 0)] = new Pieces.Queen { Color = TeamColor.White };
            board[new Coordinate(4, 0)] = new Pieces.King { Color = TeamColor.White };
            board[new Coordinate(5, 0)] = new Pieces.Bishop { Color = TeamColor.White };
            board[new Coordinate(6, 0)] = new Pieces.Knight { Color = TeamColor.White };
            board[new Coordinate(7, 0)] = new Pieces.Rook { Color = TeamColor.White };

            board[new Coordinate(0, 1)] = new Pieces.Pawn { Color = TeamColor.White };
            board[new Coordinate(1, 1)] = new Pieces.Pawn { Color = TeamColor.White };
            board[new Coordinate(2, 1)] = new Pieces.Pawn { Color = TeamColor.White };
            board[new Coordinate(3, 1)] = new Pieces.Pawn { Color = TeamColor.White };
            board[new Coordinate(4, 1)] = new Pieces.Pawn { Color = TeamColor.White };
            board[new Coordinate(5, 1)] = new Pieces.Pawn { Color = TeamColor.White };
            board[new Coordinate(6, 1)] = new Pieces.Pawn { Color = TeamColor.White };
            board[new Coordinate(7, 1)] = new Pieces.Pawn { Color = TeamColor.White };

            board[new Coordinate(0, 7)] = new Pieces.Rook { Color = TeamColor.Black };
            board[new Coordinate(1, 7)] = new Pieces.Knight { Color = TeamColor.Black };
            board[new Coordinate(2, 7)] = new Pieces.Bishop { Color = TeamColor.Black };
            board[new Coordinate(3, 7)] = new Pieces.Queen { Color = TeamColor.Black };
            board[new Coordinate(4, 7)] = new Pieces.King { Color = TeamColor.Black };
            board[new Coordinate(5, 7)] = new Pieces.Bishop { Color = TeamColor.Black };
            board[new Coordinate(6, 7)] = new Pieces.Knight { Color = TeamColor.Black };
            board[new Coordinate(7, 7)] = new Pieces.Rook { Color = TeamColor.Black };

            board[new Coordinate(0, 6)] = new Pieces.Pawn { Color = TeamColor.Black };
            board[new Coordinate(1, 6)] = new Pieces.Pawn { Color = TeamColor.Black };
            board[new Coordinate(2, 6)] = new Pieces.Pawn { Color = TeamColor.Black };
            board[new Coordinate(3, 6)] = new Pieces.Pawn { Color = TeamColor.Black };
            board[new Coordinate(4, 6)] = new Pieces.Pawn { Color = TeamColor.Black };
            board[new Coordinate(5, 6)] = new Pieces.Pawn { Color = TeamColor.Black };
            board[new Coordinate(6, 6)] = new Pieces.Pawn { Color = TeamColor.Black };
            board[new Coordinate(7, 6)] = new Pieces.Pawn { Color = TeamColor.Black };

            return board;
        }
    }

}
