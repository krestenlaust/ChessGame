namespace ChessGame.Gamemodes
{
    using ChessGame.Pieces;

    /// <summary>
    /// Test implementation of a gamemode. Only pawns and kings.
    /// </summary>
    public class PawnTestChess : Gamemode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PawnTestChess"/> class.
        /// </summary>
        /// <param name="playerWhite">The player instance of player white.</param>
        /// <param name="playerBlack">The player instance of player black.</param>
        public PawnTestChess(Player playerWhite, Player playerBlack)
            : base(playerWhite, playerBlack)
        {
        }

        /// <inheritdoc/>
        public override Chessboard GenerateBoard()
        {
            Chessboard board = new Chessboard(8, 8, this);

            board[new Coordinate(4, 0)] = new King { Color = TeamColor.White };
            board[new Coordinate(4, 7)] = new King { Color = TeamColor.Black };

            board[new Coordinate(3, 3)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(4, 3)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(5, 3)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(6, 3)] = new Pawn { Color = TeamColor.White };
            board[new Coordinate(7, 3)] = new Pawn { Color = TeamColor.White };

            board[new Coordinate(0, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(1, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(2, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(3, 6)] = new Pawn { Color = TeamColor.Black };
            board[new Coordinate(4, 6)] = new Pawn { Color = TeamColor.Black };

            return board;
        }
    }
}
