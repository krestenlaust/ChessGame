namespace ChessGame.Pieces
{
    public class Pawn : Piece
    {
        public Pawn()
        {
            this.Notation = '\0'; // nothing
            this.MaterialValue = 1;
            this.MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.PromotionPattern(),
                new MovementPatterns.PawnCapturePattern(),
                new MovementPatterns.EnPassentPattern(),
                new MovementPatterns.PawnPushPattern()
            };
        }
    }
}
