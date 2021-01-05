using System.Text;

namespace ChessGame
{
    public class Move // kunne måske godt være en struct...?
    {
        public PieceMove[] Moves;
        private readonly string CustomNotation = null;

        public Move(PieceMove[] moves)
        {
            Moves = moves;
        }

        public Move(PieceMove[] moves, string notation)
        {
            Moves = moves;
            CustomNotation = notation;
        }

        public Move(Coordinate position, Piece piece, bool captures)
        {
            Moves = new PieceMove[]
            {
                new PieceMove(position, piece, captures)
            };
        }

        public Move(Coordinate position, Piece piece, bool captures, string notation)
        {
            Moves = new PieceMove[]
            {
                new PieceMove(position, piece, captures)
            };

            CustomNotation = notation;
        }

        public override string ToString()
        {
            // Return custom notation if it's defined.
            if (!(CustomNotation is null))
            {
                return CustomNotation;
            }

            StringBuilder sb = new StringBuilder();

            foreach (var singleMove in Moves)
            {
                sb.Append(',');
                sb.Append(singleMove.ToString());
            }

            return sb.ToString().Substring(1);
        }
    }
}
