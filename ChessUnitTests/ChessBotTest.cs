using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessGame;
using ChessBots;
using ChessGame.Gamemodes;

namespace ChessUnitTests
{
    [TestClass]
    public class ChessBotTest
    {
        [TestMethod]
        public void SkakinatorPerformanceTest()
        {
            // intial move
            Chessboard board = new ClassicChess(null, null).GenerateBoard();

            SkakinatorLogic logic = new SkakinatorLogic();
            logic.GenerateMove(board, 2);

            board[new Coordinate("c2")] = null;
            board[new Coordinate("d2")] = null;
            board[new Coordinate("e2")] = null;
            board[new Coordinate("f2")] = null;

            logic.GenerateMove(board, 2);
        }
    }
}
