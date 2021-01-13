using System;
using System.Collections.Generic;

namespace ChessGame
{
    public class Game
    {
        public readonly Chessboard Board;
        public TeamColor CurrentTurn = TeamColor.White;
        public Player CurrentPlayerTurn
        {
            get
            {
                return CurrentTurn == TeamColor.White ? playerWhite : playerBlack;
            }
        }
        private readonly Player playerBlack;
        private readonly Player playerWhite;
        private readonly Gamemode gamemode;
        private readonly Stack<Move> moves = new Stack<Move>();

        public Game(Player white, Player black, Gamemode gamemode)
        {
            this.gamemode = gamemode;
            playerWhite = white;
            playerBlack = black;
            Board = gamemode.GenerateBoard();
        }

        public bool MakeMove(string move)
        {
            Move pieceMove = Board.MoveByNotation(move, CurrentTurn);

            if (pieceMove is null)
                return false;

            Board.Move(pieceMove);
            moves.Push(pieceMove);
            Board.UpdateDangerzones(pieceMove);

            CurrentTurn = CurrentTurn == TeamColor.Black ? TeamColor.White : TeamColor.Black;

            return true;
        }
    }
}
