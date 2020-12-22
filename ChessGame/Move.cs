using System.Text;

namespace ChessGame
{
    public class Move // kunne måske godt være en struct...?
    {
        public PieceMove[] Moves;
        public string CustomNotation = null;

        public Move(PieceMove[] moves)
        {
            Moves = moves;
        }

        public Move(PieceMove[] moves, string notation)
        {
            Moves = moves;
            CustomNotation = notation;
        }

        public override string ToString()
        {
            if (!(CustomNotation is null))
                return CustomNotation;

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
