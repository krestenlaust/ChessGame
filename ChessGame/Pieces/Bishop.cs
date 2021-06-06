namespace ChessGame.Pieces
{
    public class Bishop : Piece
    {
        public Bishop()
        {
            this.Notation = 'B';
            this.MaterialValue = 3;
            this.MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.DiagonalPattern()
            };
        }
    }
}
