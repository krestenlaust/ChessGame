using System.Text;

namespace ChessGame
{
    /// <summary>
    /// In Chess, a coordinate consists of files and ranks, which represents respectively the x and y coordinate of a position.
    /// The file is typically expressed in letters and goes first, e.g. (0, 0) would be 'A1'.
    /// </summary>
    public readonly struct Coordinate
    {
        public readonly int File;
        public readonly int Rank;

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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append((char)(97 + File)); // tilføj bogstavs modpart af 'File'.
            sb.Append(Rank);

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
