using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Players
{
    public class Skakinator : Player
    {
        public Skakinator(string name) : base(name)
        {
        }

        private int MinimaxSearch(Chessboard board, int depth, int alpha, int beta)
        {
            if (board.CurrentState == GameState.Checkmate)
            {
                // note: the turn is updated even after checkmate
                return (depth + 1) * (board.CurrentTeamTurn == TeamColor.Black ? short.MaxValue : short.MinValue);
            }

            if (board.CurrentState == GameState.Stalemate)
            {
                return 0;
            }

            if (depth == 0)
            {
                return board.MaterialSum;
            }

            bool maximize = board.CurrentTeamTurn == TeamColor.White;
            int bestEvaluation = maximize ? int.MinValue : int.MaxValue;
            foreach (var move in board.GetMoves())
            {
                Chessboard childNode = new Chessboard(board, move);

                int newDepth;
                if (move.Captures && depth == 1)
                {
                    // check one level deeper because last move was a capture move.
                    newDepth = depth;
                }
                else
                {
                    newDepth = depth - 1;
                }

                if (maximize)
                {
                    bestEvaluation = Math.Max(bestEvaluation, MinimaxSearch(childNode, newDepth, alpha, beta));
                    alpha = Math.Max(alpha, bestEvaluation);                
                }
                else
                {
                    bestEvaluation = Math.Min(bestEvaluation, MinimaxSearch(childNode, newDepth, alpha, beta));
                    beta = Math.Min(beta, bestEvaluation);
                }

                if (beta <= alpha)
                {
                    break;
                }
            }

            return bestEvaluation;
        }

        public override void TurnStarted(Chessboard board)
        {
            List<(int, Move)> moves = new List<(int, Move)>();
            foreach (var move in board.GetMoves())
            {
                Chessboard rootNode = new Chessboard(board, move);

                moves.Add((MinimaxSearch(rootNode, 3, int.MinValue, int.MaxValue), move));
            }

            List<Move> sortedMoves;
            if (board.CurrentTeamTurn == TeamColor.White)
            {
                sortedMoves = (from moveEvaluation in moves
                               orderby moveEvaluation.Item1 descending
                               select moveEvaluation.Item2).ToList();
            }
            else
            {
                sortedMoves = (from moveEvaluation in moves
                               orderby moveEvaluation.Item1 ascending
                               select moveEvaluation.Item2).ToList();
            }

            board.PerformMove(sortedMoves[0]);
        }
    }
}
