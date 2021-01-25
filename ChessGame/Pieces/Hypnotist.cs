namespace ChessGame.Pieces
{
    public class Hypnotist : Piece
    {
        public Hypnotist()
        {
            Notation = 'H';
            MaterialValue = 5;
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.HypnoJumpPattern()
            };
        }
    }
}
