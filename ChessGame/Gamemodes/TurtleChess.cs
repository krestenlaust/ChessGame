using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Gamemodes
{
    /// <summary>
    /// Chess with regular board size and no timer.
    /// </summary>
    public class TurtleChess : Gamemode
    {
        public TurtleChess()
        {
            Name = "Unlimited Time Chess";
        }

        public override Board GenerateBoard()
        {
            Board board = new Board(8, 8);

            board[new Coordinate(0, 0)] = new Pieces.Rook();
            board[new Coordinate(1, 0)] = new Pieces.Knight();
            board[new Coordinate(2, 0)] = new Pieces.Bishop();
            board[new Coordinate(3, 0)] = new Pieces.Queen();
            board[new Coordinate(4, 0)] = new Pieces.King();
            board[new Coordinate(5, 0)] = new Pieces.Bishop();
            board[new Coordinate(6, 0)] = new Pieces.Knight();
            board[new Coordinate(7, 0)] = new Pieces.Rook();

            board[new Coordinate(0, 1)] = new Pieces.Pawn();
            board[new Coordinate(1, 1)] = new Pieces.Pawn();
            board[new Coordinate(2, 1)] = new Pieces.Pawn();
            board[new Coordinate(3, 1)] = new Pieces.Pawn();
            board[new Coordinate(4, 1)] = new Pieces.Pawn();
            board[new Coordinate(5, 1)] = new Pieces.Pawn();
            board[new Coordinate(6, 1)] = new Pieces.Pawn();
            board[new Coordinate(7, 1)] = new Pieces.Pawn();

            board[new Coordinate(0, 7)] = new Pieces.Rook();
            board[new Coordinate(1, 7)] = new Pieces.Knight();
            board[new Coordinate(2, 7)] = new Pieces.Bishop();
            board[new Coordinate(3, 7)] = new Pieces.Queen();
            board[new Coordinate(4, 7)] = new Pieces.King();
            board[new Coordinate(5, 7)] = new Pieces.Bishop();
            board[new Coordinate(6, 7)] = new Pieces.Knight();
            board[new Coordinate(7, 7)] = new Pieces.Rook();

            board[new Coordinate(0, 6)] = new Pieces.Pawn();
            board[new Coordinate(1, 6)] = new Pieces.Pawn();
            board[new Coordinate(2, 6)] = new Pieces.Pawn();
            board[new Coordinate(3, 6)] = new Pieces.Pawn();
            board[new Coordinate(4, 6)] = new Pieces.Pawn();
            board[new Coordinate(5, 6)] = new Pieces.Pawn();
            board[new Coordinate(6, 6)] = new Pieces.Pawn();
            board[new Coordinate(7, 6)] = new Pieces.Pawn();

            return board;
        }
    }
}
