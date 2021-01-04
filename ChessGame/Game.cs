using System;
using System.Collections.Generic;

namespace ChessGame
{
    public class Game
    {
        public Player PlayerBlack;
        public Player PlayerWhite;
        public readonly Gamemode Gamemode;
        public TeamColor CurrentTurn = TeamColor.White;
        public Stack<Move> Moves;
        public Board Board;

        public Game(Player white, Player black, Gamemode gamemode)
        {
            Gamemode = gamemode;
            PlayerWhite = white;
            PlayerBlack = black;
            Board = gamemode.GenerateBoard();
        }
    }
}
