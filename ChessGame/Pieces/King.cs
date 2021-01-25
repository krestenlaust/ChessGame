namespace ChessGame.Pieces
{
    public class King : Piece
    {
        public King()
        {
            Notation = 'K';
            MaterialValue = byte.MaxValue;
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.KingPattern(),
                new MovementPatterns.CastlePattern()
            };
        }
    }
}
