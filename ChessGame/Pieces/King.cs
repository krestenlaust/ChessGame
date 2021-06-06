namespace ChessGame.Pieces
{
    public class King : Piece
    {
        public King()
        {
            this.Notation = 'K';
            this.MaterialValue = byte.MaxValue;
            this.MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.KingPattern(),
                new MovementPatterns.CastlePattern()
            };
        }
    }
}
