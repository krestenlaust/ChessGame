namespace ChessGame.Pieces
{
    public class Queen : Piece
    {
        public Queen()
        {
            Notation = 'Q';
            MaterialValue = 9;
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.CardinalPattern(),
                new MovementPatterns.DiagonalPattern()
            };
        }
    }
}
