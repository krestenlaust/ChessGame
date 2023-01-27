using System.Collections.Generic;
using System.Text;

namespace ChessGame;

/// <summary>
/// Describes a complete set of submoves, the average move only contains one <c>PieceMove</c>,
/// castling e.g. contains two <c>PieceMove</c>s.
/// </summary>
public class Move : IEqualityComparer<Move>
{
    /// <summary>
    /// Returns true if any of <c>Submoves</c> captures.
    /// </summary>
    public bool Captures
    {
        get
        {
            foreach (var singleMove in Submoves)
            {
                if (singleMove.Captures)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public readonly PieceMove[] Submoves;
    public readonly string CustomNotation;
    public readonly TeamColor Color;

    /// <summary>
    /// Initializes a new instance of the <see cref="Move"/> class.
    /// </summary>
    /// <param name="submoves"></param>
    /// <param name="color"></param>
    public Move(PieceMove[] submoves, TeamColor color)
    {
        Submoves = submoves;
        Color = color;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Move"/> class.
    /// </summary>
    /// <param name="submoves"></param>
    /// <param name="notation"></param>
    /// <param name="color"></param>
    public Move(PieceMove[] submoves, string notation, TeamColor color)
    {
        Submoves = submoves;
        CustomNotation = notation;
        Color = color;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Move"/> class.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="source"></param>
    /// <param name="piece"></param>
    /// <param name="captures"></param>
    /// <param name="color"></param>
    public Move(Coordinate position, Coordinate source, Piece piece, bool captures, TeamColor color)
    {
        Submoves = new[]
        {
            new PieceMove(position, source, piece, captures),
        };
        Color = color;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Move"/> class.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="source"></param>
    /// <param name="piece"></param>
    /// <param name="captures"></param>
    /// <param name="notation"></param>
    /// <param name="color"></param>
    public Move(Coordinate position, Coordinate source, Piece piece, bool captures, string notation, TeamColor color)
    {
        Submoves = new[]
        {
            new PieceMove(position, source, piece, captures),
        };

        CustomNotation = notation;
        Color = color;
    }

    public static bool operator ==(Move a, Move b) => a.Equals(b);

    public static bool operator !=(Move a, Move b) => !a.Equals(b);

    public string ToString(MoveNotation notationType)
    {
        switch (notationType)
        {
            case MoveNotation.UCI:
                return Submoves[0].ToString(MoveNotation.UCI);
            case MoveNotation.StandardAlgebraic:
                return ToString();
            default:
                break;
        }

        return string.Empty;
    }

    public override string ToString()
    {
        // Return custom notation if it's defined.
        if (!(CustomNotation is null))
        {
            return CustomNotation;
        }

        StringBuilder sb = new StringBuilder();

        foreach (var singleMove in Submoves)
        {
            sb.Append(',');
            sb.Append(singleMove.ToString());
        }

        return sb.ToString().Substring(1);
    }

    public override bool Equals(object obj)
    {
        return obj is Move move &&
               ToString() == move.ToString() &&
               CustomNotation == move.CustomNotation &&
               Color == move.Color;
    }

    public override int GetHashCode()
    {
        int hashCode = -895752563;
        hashCode = hashCode * -1521134295 + ToString().GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CustomNotation);
        hashCode = hashCode * -1521134295 + Color.GetHashCode();
        return hashCode;
    }

    public bool Equals(Move x, Move y) => x.Equals(y);

    public int GetHashCode(Move obj) => obj.GetHashCode();
}
