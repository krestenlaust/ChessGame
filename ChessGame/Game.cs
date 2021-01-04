using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class Game
    {
        public Player PlayerBlack;
        public Player PlayerWhite;
        public readonly Gamemode Gamemode;

        public Game(Player white, Player black, Gamemode gamemode)
        {
            Gamemode = gamemode;
            PlayerWhite = white;
            PlayerBlack = black;
        }
    }
}
