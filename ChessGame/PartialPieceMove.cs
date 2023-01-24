using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    /// <summary>
    /// Result of parsed notation, used to lookup moves.
    /// </summary>
    public struct PartialPieceMove
    {
        /// <summary>
        /// If null, disappears out of the thin air.
        /// </summary>
        public readonly Coordinate? Destination;

        /// <summary>
        /// If null, appears out of the thing air.
        /// </summary>
        public readonly Coordinate? Source;

        public readonly bool Captures;
        public readonly Piece Piece;
        public readonly Piece PromotePiece;
    }
}
