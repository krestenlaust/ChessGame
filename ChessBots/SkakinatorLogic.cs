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
        public event Action<int, int, Move, float> onSingleMoveCalculated;
        private const float centerSquareModifier = 0.2f;
        private const float centerSquareRawValue = 0.5f;

        private Dictionary<Chessboard, float> transpositionTable = new Dictionary<Chessboard, float>();

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
            foreach ((Pawn, Coordinate) item in board.GetPieces<Pawn>())
            {
                int dangerzoneSum = board.GetDangerSquareSum(item.Item2);

                if (item.Item1.Color == TeamColor.White)
                {
                    if (dangerzoneSum <= 0)
                    {
                        continue;
                    }

                    centipawns += 0.05f * (item.Item2.Rank - 1);
                }
                else
                {
                    if (dangerzoneSum >= 0)
                    {
                        continue;
                    }

                    centipawns -= 0.05f * (7 - item.Item2.Rank);
                }
            }

            foreach (var item in board.Dangerzone)
            {
                if (item.Key.Rank < 3 || item.Key.Rank > 4)
                {
                    continue;
                }

                if (item.Key.File < 2 || item.Key.File > 5)
                {
                    continue;
                }

                if (item.Value is null)
                {
                    continue;
                }

                int round = board.Moves.Count + 1;
                foreach (var piece in item.Value)
                {
                    if (piece is Queen)
                    {
                        continue;
                    }

                    if (piece.Color == TeamColor.White)
                    {
                        centipawns += centerSquareRawValue / (round * centerSquareModifier);
                    }else
                    {
                        centipawns -= centerSquareRawValue / (round * centerSquareModifier);
                    }
                }
            }

            return board.MaterialSum + centipawns;
        }

        public float MinimaxSearch(Chessboard board, int depth, float alpha, float beta)
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

        /// <summary>
        /// For timing and analyzing
        /// </summary>
        /// <param name="board"></param>
        /// <param name="targetDepth"></param>
        /// <returns></returns>
        public Move GenerateMoveTimed(Chessboard board, int targetDepth)
        {
            List<(float, Move)> moves = new List<(float, Move)>();
            List<Move> availableMoves = board.GetMoves().ToList();

            foreach (var move in availableMoves)
            {
                Chessboard rootNode = new Chessboard(board, move);

                float evaluation = MinimaxSearch(rootNode, targetDepth - 1, float.MinValue, float.MaxValue);
                moves.Add((evaluation, move));
            }


            double bestEvaluation;
            if (board.CurrentTeamTurn == TeamColor.White)
            {
                bestEvaluation = moves.Max(m => m.Item1);
            }
            else
            {
                bestEvaluation = moves.Min(m => m.Item1);
            }

            bestEvaluation = Math.Round(bestEvaluation, 1);

            List<Move> sortedMoves;
            if (board.CurrentTeamTurn == TeamColor.White)
            {
                sortedMoves = (from moveEvaluation in moves
                               orderby moveEvaluation.Item1 descending
                               where Math.Round(moveEvaluation.Item1, 1) == bestEvaluation
                               select moveEvaluation.Item2).ToList();
            }
            else
            {
                sortedMoves = (from moveEvaluation in moves
                               orderby moveEvaluation.Item1 ascending
                               where Math.Round(moveEvaluation.Item1, 1) == bestEvaluation
                               select moveEvaluation.Item2).ToList();
            }

            Move chosenMove = sortedMoves[0];

            foreach (var move in sortedMoves)
            {
                string moveNotation = move.ToString();
                if (moveNotation == "O-O-O" || moveNotation == "O-O")
                {
                    chosenMove = move;
                    break;
                }
            }

            // Clean-up
            transpositionTable.Clear();

            return sortedMoves[0];
        }

        /// <summary>
        /// Generates the supposedly best move given a position on the board and a <c>targetDepth</c>.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="targetDepth"></param>
        /// <returns></returns>
        public Move GenerateMoveParrallel(Chessboard board, int targetDepth)
        {
            SemaphoreSlim ss = new SemaphoreSlim(5, 5);
            List<Task> moveTasks = new List<Task>();

            List<(float, Move)> moves = new List<(float, Move)>();
            List<Move> availableMoves = board.GetMoves().ToList();
            onSingleMoveCalculated?.Invoke(0, availableMoves.Count, null, StaticEvaluation(board, 1));

            foreach (var move in availableMoves)
            {
                Chessboard rootNode = new Chessboard(board, move);

                ss.Wait();

                moveTasks.Add(
                    Task.Run(() =>
                    {
                        float evaluation = MinimaxSearch(rootNode, targetDepth - 1, float.MinValue, float.MaxValue);
                        moves.Add((evaluation, move));
                        onSingleMoveCalculated?.Invoke(moves.Count, availableMoves.Count, move, evaluation);
                        ss.Release();
                    }
                    ));
            }

            Task.WaitAll(moveTasks.ToArray());


            double bestEvaluation;
            if (board.CurrentTeamTurn == TeamColor.White)
            {
                bestEvaluation = moves.Max(m => m.Item1);
            }
            else
            {
                bestEvaluation = moves.Min(m => m.Item1);
            }

            bestEvaluation = Math.Round(bestEvaluation, 1);

            List<Move> sortedMoves;
            if (board.CurrentTeamTurn == TeamColor.White)
            {
                sortedMoves = (from moveEvaluation in moves
                               orderby moveEvaluation.Item1 descending
                               where Math.Round(moveEvaluation.Item1, 1) == bestEvaluation
                               select moveEvaluation.Item2).ToList();
            }
            else
            {
                sortedMoves = (from moveEvaluation in moves
                               orderby moveEvaluation.Item1 ascending
                               where Math.Round(moveEvaluation.Item1, 1) == bestEvaluation
                               select moveEvaluation.Item2).ToList();
            }

            Move chosenMove = sortedMoves[0];

            foreach (var move in sortedMoves)
            {
                string moveNotation = move.ToString();
                if (moveNotation == "O-O-O" || moveNotation == "O-O")
                {
                    chosenMove = move;
                    break;
                }
            }

            // Clean-up
            transpositionTable.Clear();

            return sortedMoves[0];
        }
    }
}
    