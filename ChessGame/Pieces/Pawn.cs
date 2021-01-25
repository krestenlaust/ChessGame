namespace ChessGame.Pieces
{
    public class Pawn : Piece
    {
        public Pawn()
        {
            Notation = '\0'; // nothing
            MaterialValue = 1;
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.PromotionPattern(),
                new MovementPatterns.PawnCapturePattern(),
                new MovementPatterns.EnPassentPattern(),
                new MovementPatterns.PawnPushPattern()
            };
        }
    }
}
