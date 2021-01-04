using System.Text;

namespace ChessGame
{
    /// <summary>
    /// In Chess, a coordinate consists of files and ranks, which represents respectively the x and y coordinate of a position.
    /// The file is typically expressed in letters and goes first, e.g. (0, 0) would be 'A1'.
    /// </summary>
    public readonly struct Coordinate
    {
        /// <summary>
        /// Zero-indexed. e.g. A would be 0.
        /// </summary>
        public readonly int File;
        /// <summary>
        /// Zero-indexed. Row 1 is actually row 0.
        /// </summary>
        public readonly int Rank;

        /// <summary>
        /// Coordinate that describes the position of a piece.
        /// </summary>
        /// <param name="file">The x-coordinate, the column. Usually expressed using the letters A-G.</param>
        /// <param name="rank">The y-coordinate, the row.</param>
        public Coordinate(int file, int rank)
        {
            File = file;
            Rank = rank;
        }

        public static bool operator ==(Coordinate coordinate1, Coordinate coordinate2) => 
            coordinate1.File == coordinate2.File && coordinate1.Rank == coordinate2.Rank;

        public static bool operator !=(Coordinate coordinate1, Coordinate coordinate2) => 
            !(coordinate1 == coordinate2);

        public static Coordinate operator +(Coordinate coordinate1, Coordinate coordinate2) => new Coordinate(coordinate1.File + coordinate2.File, coordinate1.Rank + coordinate2.Rank);

        /// <summary>
        /// Converts a coordinate to one with file as letter and rank as one-indexed number.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append((char)(97 + File)); // tilføj bogstavs modpart til værdien af File.
            sb.Append(Rank + 1);

            return sb.ToString();
        }

        // autogenereret af visual studio
        public override bool Equals(object obj)
        {
            return obj is Coordinate coordinate &&
                   File == coordinate.File &&
                   Rank == coordinate.Rank;
        }
        
        // autogenereret af visual studio
        public override int GetHashCode()
        {
            int hashCode = -73919966;
            hashCode = hashCode * -1521134295 + File.GetHashCode();
            hashCode = hashCode * -1521134295 + Rank.GetHashCode();
            return hashCode;
        }
    }
}
