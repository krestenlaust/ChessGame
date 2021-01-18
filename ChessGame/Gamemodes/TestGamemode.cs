using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Gamemodes
{
    public class TestGamemode : Gamemode
    {
        public override Chessboard GenerateBoard(Player playerWhite, Player playerBlack)
        {
            Chessboard board = new Chessboard(8, 8, this, playerWhite, playerBlack);

            board[new Coordinate(1, 3)] = new Pieces.Queen();
            board[new Coordinate(3, 4)] = new Pieces.Bishop();
            board[new Coordinate(6, 4)] = new Pieces.Rook();
            board[new Coordinate(1, 1)] = new Pieces.King();
            board[new Coordinate(6, 6)] = new Pieces.King() { Color = TeamColor.White};

            return board;
        }
    }
}
