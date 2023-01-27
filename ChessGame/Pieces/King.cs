namespace ChessGame.Pieces;

public class King : Piece
{
    /// <summary>
    /// Initializes a new instance of the <see cref="King"/> class.
    /// </summary>
    /// <param name="teamColor"></param>
    public King(TeamColor teamColor)
        : base('K', byte.MaxValue, teamColor)
    {
        MovementPatternList = new IMovementPattern[]
        {
            new MovementPatterns.KingPattern(),
            new MovementPatterns.CastlePattern(),
        };
    }
}
