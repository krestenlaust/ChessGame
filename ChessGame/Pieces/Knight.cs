namespace ChessGame.Pieces
{
    public class Knight : Piece
    {
        public Knight()
        {
            this.Notation = 'N';
            this.MaterialValue = 3;
            this.MovementPatternList = new IMovementPattern[] {
                new MovementPatterns.KnightPattern()
            };
        }
    }
}
