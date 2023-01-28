namespace ChessGame;

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
