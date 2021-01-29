using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChessGame;
using ChessGame.Pieces;

namespace ChessBots
{
    /// <summary>
    /// The logic of the Skakinator.
    /// </summary>
    public class SkakinatorLogic
    {
        public event Action<int, int> onSingleMoveCalculated;

        private Dictionary<Chessboard, float> transpositionTable = new Dictionary<Chessboard, float>();
        private int totalMoveCount;

        private static float StaticEvaluation(Chessboard board, int depth)
        {
            if (board.CurrentState == GameState.Checkmate)
            {
                // note: the turn is updated even after checkmate
                return (depth + 1) * ((board.CurrentTeamTurn == TeamColor.Black ? float.MaxValue : float.MinValue) / 100);
            }

            if (board.CurrentState == GameState.Stalemate)
            {
                return 0;
            }

            float centipawns = 0;
            foreach (Pawn item in board.GetPieces<Pawn>())
            {
                if (!board.TryGetCoordinate(item, out Coordinate position))
                {
                    continue;
                }

                if (item.Color == TeamColor.White)
                {
                    centipawns += 0.05f * (position.Rank - 1);
                }
                else
                {
                    centipawns -= 0.05f * (7 - position.Rank);
                }
            }

            foreach (var item in board.Pieces)
            {
                if (item.Value is King || item.Value is Pawn || item.Value is Rook)
                {
                    continue;
                }

                if (item.Key.Rank == 0 && item.Value.Color == TeamColor.White)
                {
                    centipawns -= 0.2f;
                }
                else if (item.Key.Rank == 7 && item.Value.Color == TeamColor.Black)
                {
                    centipawns += 0.2f;
                }
            }

            return board.MaterialSum + centipawns;
        }

        private float MinimaxSearch(Chessboard board, int depth, float alpha, float beta)
        {
            if (depth == 0 || board.CurrentState == GameState.Checkmate || board.CurrentState == GameState.Stalemate)
            {
                // note: the turn is updated even after checkmate
                return StaticEvaluation(board, depth);
            }

            bool maximize = board.CurrentTeamTurn == TeamColor.White;
            float bestEvaluation = maximize ? float.MinValue : float.MaxValue;
            foreach (var move in board.GetMoves())
            {
                Chessboard childNode = new Chessboard(board, move);

                int newDepth;
                if (depth == 1 && (move.Captures || !(move.Moves[0].PromotePiece is null)))
                {
                    // check one level deeper because last move was a capture move.
                    newDepth = depth;
                }
                else
                {
                    newDepth = depth - 1;
                }

                if (transpositionTable.TryGetValue(childNode, out float precalculatedEvaluation))
                {
                    if (maximize)
                    {
                        bestEvaluation = precalculatedEvaluation;
                        alpha = Math.Max(alpha, bestEvaluation);
                    }
                    else
                    {
                        bestEvaluation = precalculatedEvaluation;
                        beta = Math.Min(beta, bestEvaluation);
                    }
                }
                else
                {
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
                }

                if (beta <= alpha)
                {
                    break;
                }
            }

            transpositionTable[board] = bestEvaluation;
            return bestEvaluation;
        }

        public Move GenerateMove(Chessboard board)
        {
            SemaphoreSlim ss = new SemaphoreSlim(5, 5);
            List<Task> moveTasks = new List<Task>();

            List<(float, Move)> moves = new List<(float, Move)>();
            List<Move> availableMoves = board.GetMoves().ToList();

            foreach (var move in availableMoves)
            {
                Chessboard rootNode = new Chessboard(board, move);

                ss.Wait();

                moveTasks.Add(
                    Task.Run(() =>
                    {
                        moves.Add((MinimaxSearch(rootNode, 2, float.MinValue, float.MaxValue), move));
                        onSingleMoveCalculated?.Invoke(moves.Count, availableMoves.Count);
                        ss.Release();
                    }
                    ));
            }

            Task.WaitAll(moveTasks.ToArray());


            float bestEvaluation;
            if (board.CurrentTeamTurn == TeamColor.White)
            {
                bestEvaluation = moves.Max(m => m.Item1);
            }
            else
            {
                bestEvaluation = moves.Min(m => m.Item1);
            }

            List<Move> sortedMoves;
            if (board.CurrentTeamTurn == TeamColor.White)
            {
                sortedMoves = (from moveEvaluation in moves
                               orderby moveEvaluation.Item1 descending
                               where moveEvaluation.Item1 == bestEvaluation
                               select moveEvaluation.Item2).ToList();
            }
            else
            {
                sortedMoves = (from moveEvaluation in moves
                               orderby moveEvaluation.Item1 ascending
                               where moveEvaluation.Item1 == bestEvaluation
                               select moveEvaluation.Item2).ToList();
            }

            // Clean-up
            transpositionTable.Clear();
            GC.Collect();

            return sortedMoves[0];
        }
    }
}
