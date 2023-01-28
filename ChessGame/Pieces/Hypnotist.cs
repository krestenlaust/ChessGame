namespace ChessGame.Pieces;

/// <summary>
/// A fairy chess piece we came up with.
/// </summary>
public class Hypnotist : Piece
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Hypnotist"/> class.
    /// </summary>
    /// <param name="teamColor"></param>
    public Hypnotist(TeamColor teamColor)
        : base('H', 5, teamColor)
    {
        MovementPatternList = new IMovementPattern[]
        {
            new MovementPatterns.HypnoJumpPattern(),
        };
    }
}
