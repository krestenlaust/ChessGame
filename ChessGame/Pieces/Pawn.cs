namespace ChessGame.Pieces;

public class Pawn : Piece
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Pawn"/> class.
    /// </summary>
    /// <param name="teamColor"></param>
    public Pawn(TeamColor teamColor)
        : base('\0', 1, teamColor)
    {
        MovementPatternList = new IMovementPattern[]
        {
            new MovementPatterns.PromotionPattern(),
            new MovementPatterns.PawnCapturePattern(),
            new MovementPatterns.EnPassentPattern(),
            new MovementPatterns.PawnPushPattern(),
        };
    }
}
