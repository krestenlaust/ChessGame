namespace ChessGame
{
    using System;
    using System.Linq;

    /// <summary>
    /// The states of a game.
    /// </summary>
    public enum GameState : byte
    {
        /// <summary>
        /// The player whose turn it is, doesn't have any available moves.
        /// </summary>
        Stalemate,

        /// <summary>
        /// The player whose turn it is, doesn't have any available moves, that unchecks thier king.
        /// </summary>
        Checkmate,

        /// <summary>
        /// The player whose turn it is, has their king in check.
        /// </summary>
        Check,

        /// <summary>
        /// The game hasn't started yet.
        /// </summary>
        NotStarted,

        /// <summary>
        /// The game has just started.
        /// </summary>
        Started,

        /// <summary>
        /// The game is in such a position that it isn't possible for any player to check the other.
        /// </summary>
        DeadPosition,
    }

    /// <summary>
    /// The abstract class representing a game. Inherit to implement gamemodes.
    /// </summary>
    public abstract class Gamemode
    {
        /// <summary>
        /// Null if no winner has been selected.
        /// </summary>
        public Player Winner;
        public readonly Player PlayerWhite;
        public readonly Player PlayerBlack;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gamemode"/> class.
        /// </summary>
        /// <param name="playerWhite">The player instance of player white.</param>
        /// <param name="playerBlack">The player instance of player black.</param>
        public Gamemode(Player playerWhite, Player playerBlack)
        {
            PlayerWhite = playerWhite;
            PlayerBlack = playerBlack;
        }

        /// <summary>
        /// Called when the internal state of the game is changed, for example on checkmate, or simply check.
        /// </summary>
        public event Action<GameState> GameStateChanged;

        /// <summary>
        /// Called when a player has finished their move and the game hasn't reached an end state.
        /// </summary>
        public event Action TurnChanged;

        /// <summary>
        /// Called to setup the chessboard.
        /// </summary>
        /// <returns>Chessboard with pieces.</returns>
        public abstract Chessboard GenerateBoard();

        /// <summary>
        /// Validates a move for a given position. Maybe merge with <c>MakeMove(Move)</c>.
        /// </summary>
        /// <param name="move">The move to verify.</param>
        /// <param name="board">The state of the board.</param>
        /// <returns>Returns whether the move is valid.</returns>
        public virtual bool ValidateMove(Move move, Chessboard board)
        {
            // if move is outside board, then it's invalid.
            foreach (var singleMove in move.Submoves)
            {
                if (singleMove.Destination is null)
                {
                    continue;
                }

                if (!board.InsideBoard(singleMove.Destination.Value))
                {
                    return false;
                }
            }

            // if move puts king in check — it's invalid.
            Chessboard boardSimulation = new Chessboard(board);
            boardSimulation.ExecuteMove(move);

            // king is in check, move is invalid
            if (boardSimulation.IsKingInCheck(board.CurrentTeamTurn))
            {
                return false;
            }

            // move is valid
            return true;
        }

        /// <summary>
        /// Updates gamestate, e.g. checks for mate/checkmate or stalemate. Run at end of turn.
        /// </summary>
        /// <returns>Whether the gamestate has updated.</returns>
        /// <param name="board">The board of which to update the gamestate.</param>
        public virtual bool UpdateGameState(Chessboard board)
        {
            GameState previousState = board.CurrentState;

            if (previousState == GameState.NotStarted)
            {
                return false;
            }

            // check for whether king is in check.
            if (board.IsKingInCheck(board.CurrentTeamTurn))
            {
                board.CurrentState = GameState.Check;
            }
            else
            {
                board.CurrentState = GameState.Started;
            }

            if (board.Pieces.Count <= 3)
            {
                bool draw = true;
                foreach (var item in board.Pieces)
                {
                    if (item.Value is Pieces.Queen || item.Value is Pieces.Rook || item.Value is Pieces.Pawn)
                    {
                        draw = false;
                    }
                }

                if (draw)
                {
                    board.CurrentState = GameState.DeadPosition;
                    return true;
                }
            }

            // no more legal moves, game is over either by stalemate or checkmate.
            if (!board.GetMoves(board.CurrentTeamTurn).Any())
            {
                // checkmate
                if (board.CurrentState == GameState.Check)
                {
                    Winner = board.CurrentTeamTurn == TeamColor.White ? PlayerBlack : PlayerWhite;
                    board.CurrentState = GameState.Checkmate;
                }
                else
                {
                    board.CurrentState = GameState.Stalemate;
                }
            }
            else if (board.CurrentState != GameState.Check)
            {
                board.CurrentState = GameState.Started;
            }

            return previousState != board.CurrentState;
        }

        /// <summary>
        /// Can be overridden to have custom turn logic.
        /// </summary>
        /// <param name="board">The current state of the board.</param>
        /// <returns>False if game has ended.</returns>
        public virtual bool StartTurn(Chessboard board)
        {
            if (UpdateGameState(board))
            {
                GameStateChanged?.Invoke(board.CurrentState);

                switch (board.CurrentState)
                {
                    case GameState.Stalemate:
                    case GameState.Checkmate:
                        return false;
                }
            }

            TurnChanged?.Invoke();
            return true;
        }
    }
}
