using System.Text;

namespace ChessGame
{
    public class Move
    {
        /// <summary>
        /// Returns true if any of <c>Moves</c> captures.
        /// </summary>
        public bool Captures
        {
            get
            {
                foreach (var singleMove in Moves)
                {
                    if (singleMove.Captures)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public readonly PieceMove[] Moves;
        public readonly string CustomNotation;
        public readonly TeamColor Color;

        public Move(PieceMove[] moves, TeamColor color)
        {
            Moves = moves;
            Color = color;
        }

        public Move(PieceMove[] moves, string notation, TeamColor color)
        {
            Moves = moves;
            CustomNotation = notation;
            Color = color;
        }

        public Move(Coordinate position, Coordinate source, Piece piece, bool captures, TeamColor color)
        {
            Moves = new []
            {
                new PieceMove(position, source, piece, captures)
            };
            Color = color;
        }

        public Move(Coordinate position, Coordinate source, Piece piece, bool captures, string notation, TeamColor color)
        {
            Moves = new []
            {
                new PieceMove(position, source, piece, captures)
            };

            CustomNotation = notation;
            Color = color;
        }

        public string ToString(MoveNotation notationType)
        {
            switch (notationType)
            {
                case MoveNotation.UCI:
                    return Moves[0].ToString(MoveNotation.UCI);
                case MoveNotation.StandardAlgebraic:
                    return ToString();
                default:
                    break;
            }

            return "";
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
