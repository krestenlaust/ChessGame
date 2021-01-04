using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public abstract class Gamemode
    {
        public readonly string Name;
        public virtual Board GetBoard() { return null; }
    }
}