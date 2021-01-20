using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessGame.Pieces;

namespace ChessGame.Gamemodes
{
    public class TinyChess : Gamemode
    {
        public override Chessboard GenerateBoard(Player playerWhite, Player playerBlack)
        {
            Chessboard board = new Chessboard(6, 8, this, playerWhite, playerBlack);

            board[new Coordinate(0, 0)] = new Rook { Color = TeamColor.White };
            board[new Coordinate(1, 0)] = new Knight { Color = TeamColor.White };
            board[new Coordinate(2, 0)] = new Bishop { Color = TeamColor.White };
            board[new Coordinate(3, 0)] = new King { Color = TeamColor.White };
            board[new Coordinate(4, 0)] = new Knight { Color = TeamColor.White };
            board[new Coordinate(5, 0)] = new Rook { Color = TeamColor.White };

            board[new Coordinate(0, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(1, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(2, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(3, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(4, 1)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(5, 1)] = new Pawn { Color = TeamColor.White };

            board[new Coordinate(0, 7)] = new Rook { Color = TeamColor.Black };
            board[new Coordinate(1, 7)] = new Knight { Color = TeamColor.Black };
            board[new Coordinate(2, 7)] = new Bishop { Color = TeamColor.Black };
            board[new Coordinate(3, 7)] = new King { Color = TeamColor.Black };
            board[new Coordinate(4, 7)] = new Knight { Color = TeamColor.Black };
            board[new Coordinate(5, 7)] = new Rook { Color = TeamColor.Black };

            board[new Coordinate(0, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(1, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(2, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(3, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(4, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(5, 6)] = new Pawn { Color = TeamColor.Black };

            return board;
        }
    }
}
