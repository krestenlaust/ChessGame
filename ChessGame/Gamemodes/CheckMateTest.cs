﻿using ChessGame.Pieces;

namespace ChessGame.Gamemodes
{
    public class CheckMateTest : Gamemode
    {
        public CheckMateTest(Player playerWhite, Player playerBlack) : base(playerWhite, playerBlack)
        {
        }

        public override Chessboard GenerateBoard()
        {
            var board = new Chessboard(8, 8, this);

            board[new Coordinate(0, 0)] = new King(TeamColor.White);
            board[new Coordinate(1, 0)] = new Rook(TeamColor.White);
            board[new Coordinate(2, 0)] = new Rook(TeamColor.White);

            board[new Coordinate(0, 7)] = new King(TeamColor.Black);

            return board;
        }
    }
}
