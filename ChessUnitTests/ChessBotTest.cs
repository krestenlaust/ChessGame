using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ChessGame;
using ChessBots;
using ChessGame.Gamemodes;

namespace ChessUnitTests
{
    [TestClass]
    public class ChessBotTest
    {
        /// <summary>
        /// Tests the performance of the Skakinator bot.
        /// </summary>
        [TestMethod]
        public void SkakinatorPerformanceTest()
        {
            // intial move
            Chessboard board = new ClassicChess(null, null).GenerateBoard();

            SkakinatorLogic logic = new SkakinatorLogic();
            logic.GenerateMoveParallel(board, 2);

            board[new Coordinate("c2")] = null;
            board[new Coordinate("d2")] = null;
            board[new Coordinate("e2")] = null;
            board[new Coordinate("f2")] = null;

            logic.GenerateMoveParallel(board, 2);
        }
    }
}
