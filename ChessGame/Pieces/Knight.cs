namespace ChessGame.Pieces;

public class Knight : Piece
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Knight"/> class.
    /// </summary>
    /// <param name="teamColor"></param>
    public Knight(TeamColor teamColor)
        : base('N', 3, teamColor)
    {
        MovementPatternList = new IMovementPattern[]
        {
            new MovementPatterns.KnightPattern(),
        };
    }
}
