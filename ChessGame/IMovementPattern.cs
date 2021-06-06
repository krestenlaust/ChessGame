namespace ChessGame
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes a specific movement pattern of a piece.
    /// </summary>
    public interface IMovementPattern
    {
        // TODO: anyCaptureOnly should probably be turned into something that just makes a piece return all moves possible.

        /// <summary>
        /// Returns the moves a piece can make based on the <c>board</c>.
        /// </summary>
        /// <param name="piece">The piece's perspective from which moves are to be calculated.</param>
        /// <param name="position">The position of the piece.</param>
        /// <param name="board">The state of the board.</param>
        /// <param name="guardedSquaresOnly">Return squares that are being guarded (<c>isCapture</c> is irrelevant).</param>
        /// <returns>Return generator of available moves.</returns>
        IEnumerable<Move> GetMoves(Piece piece, Coordinate position, Chessboard board, bool guardedSquaresOnly = false);
    }
}