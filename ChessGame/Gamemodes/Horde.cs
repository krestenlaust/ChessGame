using ChessGame.Pieces;

namespace ChessGame.Gamemodes;

public class Horde : Gamemode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Horde"/> class.
    /// </summary>
    /// <param name="playerWhite"></param>
    /// <param name="playerBlack"></param>
    public Horde(Player playerWhite, Player playerBlack)
        : base(playerWhite, playerBlack)
    {
    }

    public override Chessboard GenerateBoard()
    {
        var board = new Chessboard(8, 8, this);

        board[new Coordinate(1, 4)] = new Pawn(TeamColor.White);
        board[new Coordinate(2, 4)] = new Pawn(TeamColor.White);
        board[new Coordinate(5, 4)] = new Pawn(TeamColor.White);
        board[new Coordinate(6, 4)] = new Pawn(TeamColor.White);

        board[new Coordinate(0, 3)] = new Pawn(TeamColor.White);
        board[new Coordinate(1, 3)] = new Pawn(TeamColor.White);
        board[new Coordinate(2, 3)] = new Pawn(TeamColor.White);
        board[new Coordinate(3, 3)] = new Pawn(TeamColor.White);
        board[new Coordinate(4, 3)] = new Pawn(TeamColor.White);
        board[new Coordinate(5, 3)] = new Pawn(TeamColor.White);
        board[new Coordinate(6, 3)] = new Pawn(TeamColor.White);
        board[new Coordinate(7, 3)] = new Pawn(TeamColor.White);

        board[new Coordinate(0, 2)] = new Pawn(TeamColor.White);
        board[new Coordinate(1, 2)] = new Pawn(TeamColor.White);
        board[new Coordinate(2, 2)] = new Pawn(TeamColor.White);
        board[new Coordinate(3, 2)] = new Pawn(TeamColor.White);
        board[new Coordinate(4, 2)] = new Pawn(TeamColor.White);
        board[new Coordinate(5, 2)] = new Pawn(TeamColor.White);
        board[new Coordinate(6, 2)] = new Pawn(TeamColor.White);
        board[new Coordinate(7, 2)] = new Pawn(TeamColor.White);

        board[new Coordinate(0, 1)] = new Pawn(TeamColor.White);
        board[new Coordinate(1, 1)] = new Pawn(TeamColor.White);
        board[new Coordinate(2, 1)] = new Pawn(TeamColor.White);
        board[new Coordinate(3, 1)] = new Pawn(TeamColor.White);
        board[new Coordinate(4, 1)] = new Pawn(TeamColor.White);
        board[new Coordinate(5, 1)] = new Pawn(TeamColor.White);
        board[new Coordinate(6, 1)] = new Pawn(TeamColor.White);
        board[new Coordinate(7, 1)] = new Pawn(TeamColor.White);

        board[new Coordinate(0, 0)] = new Pawn(TeamColor.White);
        board[new Coordinate(1, 0)] = new Pawn(TeamColor.White);
        board[new Coordinate(2, 0)] = new Pawn(TeamColor.White);
        board[new Coordinate(3, 0)] = new Pawn(TeamColor.White);
        board[new Coordinate(4, 0)] = new Pawn(TeamColor.White);
        board[new Coordinate(5, 0)] = new Pawn(TeamColor.White);
        board[new Coordinate(6, 0)] = new Pawn(TeamColor.White);
        board[new Coordinate(7, 0)] = new Pawn(TeamColor.White);

        board[new Coordinate(0, 7)] = new Rook(TeamColor.Black);
        board[new Coordinate(1, 7)] = new Knight(TeamColor.Black);
        board[new Coordinate(2, 7)] = new Bishop(TeamColor.Black);
        board[new Coordinate(3, 7)] = new Queen(TeamColor.Black);
        board[new Coordinate(4, 7)] = new King(TeamColor.Black);
        board[new Coordinate(5, 7)] = new Bishop(TeamColor.Black);
        board[new Coordinate(6, 7)] = new Knight(TeamColor.Black);
        board[new Coordinate(7, 7)] = new Rook(TeamColor.Black);

        board[new Coordinate(0, 6)] = new Pawn(TeamColor.Black);
        board[new Coordinate(1, 6)] = new Pawn(TeamColor.Black);
        board[new Coordinate(2, 6)] = new Pawn(TeamColor.Black);
        board[new Coordinate(3, 6)] = new Pawn(TeamColor.Black);
        board[new Coordinate(4, 6)] = new Pawn(TeamColor.Black);
        board[new Coordinate(5, 6)] = new Pawn(TeamColor.Black);
        board[new Coordinate(6, 6)] = new Pawn(TeamColor.Black);
        board[new Coordinate(7, 6)] = new Pawn(TeamColor.Black);

        return board;
    }
}
