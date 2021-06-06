namespace ChessGame.Pieces
{
    /// <summary>
    /// A fairy chess piece we came up with.
    /// </summary>
    public class Hypnotist : Piece
    {
        public Hypnotist()
        {
            this.Notation = 'H';
            this.MaterialValue = 5;
            this.MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.HypnoJumpPattern()
            };
        }
    }
}
