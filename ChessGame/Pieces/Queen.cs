namespace ChessGame.Pieces
{
    public class Queen : Piece
    {
        public Queen()
        {
            this.Notation = 'Q';
            this.MaterialValue = 9;
            this.MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.CardinalPattern(),
                new MovementPatterns.DiagonalPattern()
            };
        }
    }
}
