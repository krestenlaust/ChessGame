using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ChessGame;
using ChessGame.Gamemodes;
using ChessGame.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace ChessUnitTests
{
    [TestClass]
    public class ClassicChessTest
    {
        [TestMethod]
        public void CheckPatterns()
        {
            Player player1 = new Player("player1");
            Player player2 = new Player("player2");
            Gamemode gamemode = new ClassicChess(player1, player2);

            // https://lichess.org/editor/8/8/8/8/8/2pp4/8/k3K3_w_-_-_0_1
            Chessboard chessboard = new Chessboard(8, 8, gamemode);
            chessboard[new Coordinate(0, 0)] = new King(TeamColor.Black);
            chessboard[new Coordinate(4, 0)] = new King(TeamColor.White);
            chessboard[new Coordinate(2, 2)] = new Pawn(TeamColor.Black);
            chessboard[new Coordinate(3, 2)] = new Pawn(TeamColor.Black);

            chessboard.StartNextTurn();
            chessboard.CurrentTeamTurn = TeamColor.Black;
            chessboard.PerformMove("d3d2", MoveNotation.UCI);

            // check if checkstate updates.
            Assert.AreEqual(chessboard.CurrentState, GameState.Check);

            // list of actual possible moves.
            List<Coordinate> possibleMoves = new List<Coordinate>
            {
                new Coordinate("d1"),
                new Coordinate("e2"),
                new Coordinate("f2"),
                new Coordinate("f1")
            };
            List<Move> moves = chessboard.GetMoves().ToList();

            // check if the amount of moves even is correct.
            Assert.IsTrue(moves.Count == possibleMoves.Count);

            // checks if all moves are valid.
            foreach (var item in moves)
            {
                Assert.IsNotNull(item.Moves[0].Destination);
                Assert.IsTrue(possibleMoves.Contains(item.Moves[0].Destination.Value));
                Assert.IsFalse(item.Captures);
            }

            // next move
            chessboard.PerformMove("e1d1", MoveNotation.UCI);
            chessboard.PerformMove("c3c2", MoveNotation.UCI);
            // new position: https://lichess.org/analysis/8/8/8/8/8/2pp4/8/k3K3_b_-_-_0_1#4

            Assert.AreEqual(chessboard.CurrentState, GameState.Check);

            possibleMoves = new List<Coordinate>
            {
                new Coordinate("e2"),
                new Coordinate("d2"),
                new Coordinate("c2")
            };
            moves = chessboard.GetMoves().ToList();

            Assert.IsTrue(moves.Count == possibleMoves.Count);

            foreach (var item in moves)
            {
                Assert.IsNotNull(item.Moves[0].Destination);
                Assert.IsTrue(possibleMoves.Contains(item.Moves[0].Destination.Value));
            }

            // next move
            chessboard.PerformMove("d1d2", MoveNotation.UCI);
            chessboard.PerformMove("a1b2", MoveNotation.UCI);
            // new position: https://lichess.org/analysis/8/8/8/8/8/2pp4/8/k3K3_b_-_-_0_1#6

            Assert.AreEqual(chessboard.CurrentState, GameState.Started);

            possibleMoves = new List<Coordinate>
            {
                new Coordinate("e1"),
                new Coordinate("e2"),
                new Coordinate("e3"),
                new Coordinate("d3")
            };
            moves = chessboard.GetMoves().ToList();

            Assert.IsTrue(moves.Count == possibleMoves.Count);

            foreach (var item in moves)
            {
                Assert.IsNotNull(item.Moves[0].Destination);
                Assert.IsTrue(possibleMoves.Contains(item.Moves[0].Destination.Value));
                Assert.IsFalse(item.Captures);
            }

            Assert.AreEqual(chessboard.CurrentState, GameState.Started);
        }

        [TestMethod]
        public void CoordinateNotationParsing()
        {
            Coordinate testCoord = new Coordinate("a1");

            Assert.AreNotEqual(testCoord, new Coordinate(1, 0));
            Assert.AreNotEqual(testCoord, new Coordinate(1, 1));
            Assert.AreEqual(testCoord, new Coordinate(0, 0));
            Assert.AreEqual(testCoord, new Coordinate("a1"));

            Assert.AreEqual(new Coordinate("h8"), new Coordinate(7, 7));
        }
    }
}
