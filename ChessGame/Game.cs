using System;
using System.Collections.Generic;

namespace ChessGame
{
    public class Game
    {
        public readonly Board Board;
        private TeamColor CurrentTurn = TeamColor.White;
        private readonly Player PlayerBlack;
        private readonly Player PlayerWhite;
        private readonly Gamemode Gamemode;
        private readonly Stack<Move> Moves = new Stack<Move>();

        public Game(Player white, Player black, Gamemode gamemode)
        {
            Gamemode = gamemode;
            PlayerWhite = white;
            PlayerBlack = black;
            Board = gamemode.GenerateBoard();
        }
    }
}
