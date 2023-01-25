namespace ChessGame.Bots
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ChessGame;
    using ChessGame.Pieces;

    /// <summary>
    /// The logic of the Skakinator.
    /// </summary>
    public class SkakinatorLogic
    {
        const float CenterSquareModifier = 0.2f;
        const float CenterSquareRawValue = 0.5f;
        readonly Dictionary<Chessboard, float> transpositionTable = new Dictionary<Chessboard, float>();

        /// <summary>
        /// Called once a move has been calculated.
        /// </summary>
        public event Action<int, int, Move, float> SingleMoveCalculated;

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
                if (depth == 1 && (move.Captures || !(move.Submoves[0].PromotePiece is null)))
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
                    bestEvaluation = precalculatedEvaluation;
                }
                else
                {
                    float newEvaluation = MinimaxSearch(childNode, newDepth, alpha, beta);

                    if (maximize)
                    {
                        bestEvaluation = Math.Max(bestEvaluation, newEvaluation);
                    }
                    else
                    {
                        bestEvaluation = Math.Min(bestEvaluation, newEvaluation);
                    }
                }

                if (maximize)
                {
                    alpha = Math.Max(alpha, bestEvaluation);
                }
                else
                {
                    beta = Math.Min(beta, bestEvaluation);
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
        /// For timing and analyzing/debugging.
        /// </summary>
        /// <param name="board">The state of the board.</param>
        /// <param name="targetDepth">The depth at which to search for moves. The depth of moves to simulate.</param>
        /// <returns>The 'best' move.</returns>
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
        /// Generates the supposedly best move given a position on the board and a <c>targetDepth</c>. Distributes calculations to multiple threads at once.
        /// </summary>
        /// <param name="board">The state of the board.</param>
        /// <param name="targetDepth">The amount of moves, in depth, to simulate.</param>
        /// <returns>The 'supposedly best' move.</returns>
        public Move GenerateMoveParallel(Chessboard board, int targetDepth)
        {
            SemaphoreSlim ss = new SemaphoreSlim(5, 5);
            List<Task> moveTasks = new List<Task>();

            List<(float, Move)> moves = new List<(float, Move)>();
            List<Move> availableMoves = board.GetMoves().ToList();
            SingleMoveCalculated?.Invoke(0, availableMoves.Count, null, StaticEvaluation(board, 1));

            foreach (var move in availableMoves)
            {
                Chessboard rootNode = new Chessboard(board, move);

                ss.Wait();

                moveTasks.Add(
                    Task.Run(() =>
                    {
                        float evaluation = MinimaxSearch(rootNode, targetDepth - 1, float.MinValue, float.MaxValue);
                        moves.Add((evaluation, move));
                        SingleMoveCalculated?.Invoke(moves.Count, availableMoves.Count, move, evaluation);
                        ss.Release();
                    }));
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

            // Sorts all calculated moves by their evaluation in acending order
            List<Move> sortedMoves = (from moveEvaluation in moves
                                      orderby moveEvaluation.Item1
                                      where Math.Round(moveEvaluation.Item1, 1) == bestEvaluation
                                      select moveEvaluation.Item2).ToList();

            Move chosenMove = board.CurrentTeamTurn == TeamColor.White ? sortedMoves.Last() : sortedMoves.First();

            // This is probably neccesary.
            if (board.CurrentTeamTurn == TeamColor.White)
            {
                sortedMoves.Reverse();
            }

            // TODO: find out what the intention was here.
            foreach (var move in sortedMoves)
            {
                string moveNotation = move.ToString();

                // TODO: And here.
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

        static float StaticEvaluation(Chessboard board, int depth)
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

            float hectocentipawns = 0;

            // TODO: White bias.
            // Pawn advancement-incentive.
            foreach ((Pawn, Coordinate) item in board.GetPieces<Pawn>())
            {
                int dangerzoneSum = board.GetDangerSquareSum(item.Item2);

                if (item.Item1.Color == TeamColor.White)
                {
                    if (dangerzoneSum <= 0)
                    {
                        continue;
                    }

                    hectocentipawns += 0.05f * (item.Item2.Rank - 1);
                }
                else
                {
                    if (dangerzoneSum >= 0)
                    {
                        continue;
                    }

                    hectocentipawns -= 0.05f * (7 - item.Item2.Rank);
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
                        hectocentipawns += CenterSquareRawValue / (round * CenterSquareModifier);
                    }
                    else
                    {
                        hectocentipawns -= CenterSquareRawValue / (round * CenterSquareModifier);
                    }
                }
            }

            return board.MaterialSum + hectocentipawns;
        }
    }
}
