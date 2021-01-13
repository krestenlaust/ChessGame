namespace ChessGame.Pieces
{
    public class Pawn : Piece
    {
        public Pawn()
        {
            Notation = '\0'; // nothing
            MaterialValue = 1;
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.PawnCapturePattern(),
                new MovementPatterns.PawnPushPattern(),
                new MovementPatterns.EnPassentPattern() };
        }
    }
}
