using System.Collections.Generic;

namespace ChessGame
{
    public class Game
    {
        public readonly Chessboard Board;
        public TeamColor CurrentTurn = TeamColor.Black; // changes on next turn start
        public Player CurrentPlayerTurn
        {
            get
            {
                return CurrentTurn == TeamColor.White ? playerWhite : playerBlack;
            }
        }
        private readonly Player playerBlack;
        private readonly Player playerWhite;
        private readonly Gamemode gamemode;
        private readonly Stack<Move> moves = new Stack<Move>();

        public Game(Player white, Player black, Gamemode gamemode)
        {
            this.gamemode = gamemode;
            playerWhite = white;
            playerBlack = black;
            Board = gamemode.GenerateBoard();
        }

        public void StartGame()
        {
            StartNextTurn();
        }

        public bool MakeMove(string move)
        {
            Move pieceMove = Board.MoveByNotation(move, CurrentTurn);

            if (pieceMove is null)
                return false;

            return MakeMove(pieceMove);
        }

        public bool MakeMove(Move move)
        {
            // make the actual move change the chessboard state.
            Board.Move(move);
            // add the move to the list of moves.
            moves.Push(move);

            StartNextTurn();

            return true;
        }

        private void StartNextTurn()
        {
            // refresh dangersquares
            Board.UpdateDangerzones();

            // change turn
            CurrentTurn = CurrentTurn == TeamColor.Black ? TeamColor.White : TeamColor.Black;

            CurrentPlayerTurn.TurnStarted(Board);
        }
    }
}
