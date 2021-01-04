using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Gamemodes
{
    public class TurtleChess :  Gamemode
    {
        public TurtleChess()
        {
            Name = "Unlimited Time Chess";
        }

        public override Board GetBoard()
        {
            return null;
        }
    }
}
