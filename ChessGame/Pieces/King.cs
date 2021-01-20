namespace ChessGame.Pieces
{
    public class King : Piece
    {
        public King()
        {
            Notation = 'K';
            MaterialValue = short.MaxValue;
            MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.KingPattern(),
                new MovementPatterns.CastlePattern()
            };
        }
    }
}
