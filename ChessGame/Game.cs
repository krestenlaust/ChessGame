using System;
using System.Collections.Generic;

namespace ChessGame
{
    public class Game
    {
        public readonly Player PlayerBlack;
        public readonly Player PlayerWhite;
        public readonly Gamemode Gamemode;
        public readonly Stack<Move> Moves = new Stack<Move>();
        public readonly Board Board;
        public TeamColor CurrentTurn = TeamColor.White;

        public Game(Player white, Player black, Gamemode gamemode)
        {
            Gamemode = gamemode;
            PlayerWhite = white;
            PlayerBlack = black;
            Board = gamemode.GenerateBoard();
        }
    }
}
