using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public abstract class Gamemode
    {
        public string Name { get; protected set; }
        public abstract Board GenerateBoard();
    }
}