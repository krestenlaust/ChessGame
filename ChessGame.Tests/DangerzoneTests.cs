using ChessGame.Gamemodes;
using ChessGame.Pieces;

namespace ChessGame.Tests;

[TestClass]
public class DangerzoneTests
{
    [TestMethod]
    public void TestPawnDangerzone()
    {
        Player white = new Player("white");
        Player black = new Player("black");
        Gamemode gamemode = new ClassicChess(white, black);

        // https://lichess.org/editor/8/8/8/8/8/2pp4/8/k6K_w_-_-_0_1?color=white
        Chessboard chessboard = new Chessboard(8, 8, gamemode);

        King blackKing = new King(TeamColor.Black);
        Pawn leftPawn = new Pawn(TeamColor.Black);
        Pawn rightPawn = new Pawn(TeamColor.Black);
        chessboard[new Coordinate(0, 0)] = blackKing;
        chessboard[new Coordinate(7, 0)] = new King(TeamColor.White);
        chessboard[new Coordinate(2, 2)] = leftPawn;
        chessboard[new Coordinate(3, 2)] = rightPawn;

        // Start game and refresh dangerzones.
        chessboard.StartNextTurn();

        // - Check dangerzones
        ICollection<Piece> piecesAimingAtB2 = chessboard.Dangerzone[new Coordinate(1, 1)];

        // Black king aiming at B2.
        Assert.IsTrue(piecesAimingAtB2.Contains(blackKing));

        // Right pawn aiming at B2.
        Assert.IsTrue(piecesAimingAtB2.Contains(leftPawn));

        chessboard.CurrentTeamTurn = TeamColor.Black;
        chessboard.PerformMove("c3c2", MoveNotation.UCI);

        // - Check dangerzones
        piecesAimingAtB2 = chessboard.Dangerzone[new Coordinate(1, 1)];

        // Black king aiming at B2.
        Assert.IsTrue(piecesAimingAtB2.Contains(blackKing));

        // Right pawn aiming at B2. Should be removed as pawn has moved.
        Assert.IsFalse(piecesAimingAtB2.Contains(leftPawn));
    }

    [TestMethod]
    public void TestPawnPromotionDangerzone()
    {
        Player white = new Player("white");
        Player black = new Player("black");
        Gamemode gamemode = new ClassicChess(white, black);

        // https://lichess.org/editor/8/8/8/8/8/3p4/2p5/k6K_w_-_-_0_1?color=white
        Chessboard chessboard = new Chessboard(8, 8, gamemode);

        chessboard[new Coordinate(0, 0)] = new King(TeamColor.Black);
        chessboard[new Coordinate(7, 0)] = new King(TeamColor.White);

        chessboard[new Coordinate(2, 1)] = new Pawn(TeamColor.Black);
        chessboard[new Coordinate(3, 2)] = new Pawn(TeamColor.Black);

        // Start game and refresh dangerzones.
        chessboard.StartNextTurn();

        chessboard.CurrentTeamTurn = TeamColor.Black;
        chessboard.PerformMove("c2c1R", MoveNotation.UCI);

        ICollection<Piece> piecesAimingAtE1 = chessboard.Dangerzone[new Coordinate(4, 0)];

        // Rook aiming at E1.
        Assert.IsTrue(piecesAimingAtE1.Contains(chessboard[new Coordinate(2, 0)]));
    }
}
