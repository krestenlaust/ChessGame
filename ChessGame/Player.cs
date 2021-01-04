using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class Player
    {
        public string Nickname;
        public int Wins;
        public int Losses;
        public int Draws;

        public Player(string name)
        {
            Nickname = name;
        }
    }
}
