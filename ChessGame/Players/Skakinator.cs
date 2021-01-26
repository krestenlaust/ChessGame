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

            if (board.CurrentTeamTurn == TeamColor.White)
            {
                int maxEvaluation = int.MinValue;

                foreach (var move in board.GetMoves())
                {
                    Chessboard childNode = new Chessboard(board, move);

                    maxEvaluation = Math.Max(maxEvaluation, MinimaxSearch(childNode, depth - 1, alpha, beta));
                    alpha = Math.Max(alpha, maxEvaluation);

                    // a better move was found earlier.
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return maxEvaluation;
            }
            else
            {
                int minEvaluation = int.MaxValue;

                foreach (var move in board.GetMoves())
                {
                    Chessboard childNode = new Chessboard(board, move);

                    minEvaluation = Math.Min(minEvaluation, MinimaxSearch(childNode, depth - 1, alpha, beta));
                    beta = Math.Min(beta, minEvaluation);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return minEvaluation;
            }
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
