namespace ChessGame.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(TeamColor teamColor) : base('\0', 1, teamColor)
        {
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.PromotionPattern(),
                new MovementPatterns.PawnCapturePattern(),
                new MovementPatterns.EnPassentPattern(),
                new MovementPatterns.PawnPushPattern()
            };
        }
    }
}
