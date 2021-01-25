namespace ChessGame.Pieces
{
    public class Knight : Piece
    {
        public Knight()
        {
            Notation = 'N';
            MaterialValue = 3;
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.KnightPattern()
            };
        }
    }
}
