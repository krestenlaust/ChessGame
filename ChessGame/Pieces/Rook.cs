namespace ChessGame.Pieces
{
    public class Rook : Piece
    {
        public Rook()
        {
            Notation = 'R';
            MaterialValue = 5;
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.CardinalPattern()
            };
        }
    }
}
