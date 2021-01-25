using System;
using System.Diagnostics;
using System.Text;

namespace ChessGame
{
    /// <summary>
    /// In Chess, a coordinate consists of files and ranks, which represents respectively the x and y coordinate of a position.
    /// The file is typically expressed in letters and goes first, e.g. (0, 0) would be 'A1'.
    /// </summary>
    public readonly struct Coordinate : IEquatable<Coordinate>
    {
        /// <summary>
        /// Zero-indexed. e.g. A would be 0.
        /// </summary>
        public readonly byte File;
        /// <summary>
        /// Zero-indexed. Row 1 is actually row 0.
        /// </summary>
        public readonly byte Rank;

        /// <summary>
        /// Coordinate that describes the position of a piece. Null can be used to show ambiguity (not implemented).
        /// </summary>
        /// <param name="file">The x-coordinate, the column. Usually expressed using the letters A-G.</param>
        /// <param name="rank">The y-coordinate, the row.</param>
        public Coordinate(byte file, byte rank)
        {
            File = file;
            Rank = rank;
        }

        public Coordinate(int file, int rank)
        {
            File = (byte)file;
            Rank = (byte)rank;
        }

        /// <summary>
        /// Converts coordinate notation into <c>Coordinate</c>.
        /// </summary>
        /// <param name="notation"></param>
        public Coordinate(string notation)
        {
            File = (byte)(char.ToLower(notation[0]) - 97);
            Rank = (byte)(notation[1] - 49);
        }

        /* // proposed null functionality
        [DebuggerStepThrough]
        public static bool operator ==(Coordinate coordinate1, Coordinate coordinate2)
        {
            // this is way more complicated than I imagined,
            // it's supposed to return true if all non-null equals withn their property group
            // E.g.
            // (null, 5) == (null, 5) == true
            // (null, 5) == (3, 5) == true
            // (null, null) == (3, 5) == false
            // (null, 5) == (5, null) == false
            // logic:
            // (null, n) == (null, n) == true
            // (n1, n2) == (n1, n2) == true
            // (n1, null) == (n1, n2) == true

            // all are ambigous, no number is the same.
            if (coordinate1.File is null && coordinate1.Rank is null || coordinate2.File is null && coordinate2.Rank is null)
            {
                return false;
            }

            // file is null, compare rank.
            if (coordinate1.File is null)
            {
                return coordinate1.Rank == coordinate2.Rank;
            }
            else if (coordinate1.Rank is null) // rank is null, compare file.
            {
                return coordinate1.File == coordinate1.Rank;
            }

            return coordinate1.File == coordinate2.File && coordinate1.Rank == coordinate2.Rank;
        }*/

        [DebuggerStepThrough]
        public static bool operator !=(Coordinate coordinate1, Coordinate coordinate2) => coordinate1.Rank != coordinate2.Rank || coordinate1.File != coordinate2.File;

        [DebuggerStepThrough]
        public static bool operator ==(Coordinate coordinate1, Coordinate coordinate2) => coordinate1.Rank == coordinate2.Rank && coordinate1.File == coordinate2.File;

        [DebuggerStepThrough]
        public static Coordinate operator +(Coordinate coordinate1, Coordinate coordinate2) => new Coordinate((byte)(coordinate1.File + coordinate2.File), (byte)(coordinate1.Rank + coordinate2.Rank));
        [DebuggerStepThrough]
        public static Coordinate operator -(Coordinate coordinate1, Coordinate coordinate2) => new Coordinate((byte)(coordinate1.File - coordinate2.File), (byte)(coordinate1.Rank - coordinate2.Rank));

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
        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return obj is Coordinate coordinate &&
                   File == coordinate.File &&
                   Rank == coordinate.Rank;
        }

        // autogenereret af visual studio
        [DebuggerStepThrough]
        public override int GetHashCode()
        {
            int hashCode = -73919966;
            hashCode = hashCode * -1521134295 + File.GetHashCode();
            hashCode = hashCode * -1521134295 + Rank.GetHashCode();
            return hashCode;
        }

        [DebuggerStepThrough]
        public bool Equals(Coordinate other) => other == this;
    }
}
