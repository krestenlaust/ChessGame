namespace ChessGame.Pieces
{
    public class Rook : Piece
    {
        public Rook()
        {
            this.Notation = 'R';
            this.MaterialValue = 5;
            this.MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.CardinalPattern()
            };
        }
    }
}
