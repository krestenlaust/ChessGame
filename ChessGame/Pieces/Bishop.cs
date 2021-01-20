namespace ChessGame.Pieces
{
    public class Bishop : Piece
    {
        public Bishop()
        {
            Notation = 'B';
            MaterialValue = 3;
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.DiagonalPattern()
            };
        }
    }
}
