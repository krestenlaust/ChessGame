using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public readonly struct Coordinate
    {
        public readonly int File;
        public readonly int Rank;

        public Coordinate(int file, int rank)
        {
            File = file;
            Rank = rank;
        }
    }
}
