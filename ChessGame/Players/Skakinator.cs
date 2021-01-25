using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame.Players
{
    public class Skakinator : Player
    {
        private TreeNode rootNode;

        public Skakinator(string name) : base(name)
        {
        }

        private class TreeNode
        {
            public readonly Chessboard Chessboard;
            public int? Value = null;
            private readonly Dictionary<Move, TreeNode> children;

            public bool Maximize
            {
                get
                {
                    return Chessboard.CurrentTeamTurn == TeamColor.White;
                }
            }
            public bool LeafNode
            {
                get
                {
                    switch (Chessboard.CurrentState)
                    {
                        case GameState.Stalemate:
                        case GameState.Checkmate:
                            return true;
                    }

                    return false;
                }
            }

            public TreeNode(Chessboard board)
            {
                children = new Dictionary<Move, TreeNode>();
                Chessboard = board;
                Value = null;
            }

            public TreeNode this[Move move]
            {
                get
                {

                    return children[move];
                }
                set { children[move] = value; }
            }

            public IEnumerable<TreeNode> GetChildren()
            {
                if (children.Count == 0)
                {
                    foreach (var move in Chessboard.GetMoves())
                    {
                        TreeNode node = new TreeNode(new Chessboard(Chessboard, move));
                        children[move] = node;
                    }
                }

                foreach (var node in children.Values)
                {
                    yield return node;
                }
            }

            public int Evaluation(int depth)
            {
                if (Chessboard.CurrentState == GameState.Checkmate)
                {
                    // note: the turn is updated even after checkmate
                    return (depth + 1) * (Chessboard.CurrentTeamTurn == TeamColor.Black ? short.MaxValue : short.MinValue);
                }

                if (Chessboard.CurrentState == GameState.Stalemate)
                {
                    return 0;
                }

                return Chessboard.MaterialSum;
            }
        }

        private int MinimaxSearch(TreeNode node, int depth, int alpha, int beta)
        {
            // Return node value if it's a leaf node.
            if (depth == 0 || node.LeafNode)
            {
                return node.Evaluation(depth);
            }

            int bestEvaluation = node.Maximize ? int.MinValue : int.MaxValue;

            foreach (var childNode in node.GetChildren())
            {
                if (node.Maximize)
                {
                    bestEvaluation = Math.Max(bestEvaluation, MinimaxSearch(childNode, depth - 1, alpha, beta));
                    alpha = Math.Max(alpha, bestEvaluation);
                }
                else
                {
                    bestEvaluation = Math.Min(bestEvaluation, MinimaxSearch(childNode, depth - 1, alpha, beta));
                    alpha = Math.Min(alpha, bestEvaluation);
                }

                if (beta <= alpha)
                {
                    break;
                }
            }

            node.Value = bestEvaluation;
            return bestEvaluation;
        }

        public override void TurnStarted(Chessboard board)
        {
            // first move
            if (rootNode is null)
            {
                rootNode = new TreeNode(board);
            }
            else
            {
                // move child node up tree and discard siblings.
                rootNode = rootNode[board.Moves.Peek()];
                GC.Collect();
            }

            int bestEvaluation = rootNode.Maximize ? int.MinValue : int.MaxValue;

            List<(Move, int)> evaluatedMoves = new List<(Move, int)>();
            foreach (var move in board.GetMoves())
            {
                Chessboard newBoard = new Chessboard(board, move);
                TreeNode node = new TreeNode(newBoard);

                int evaluation = MinimaxSearch(node, 3, int.MinValue, int.MaxValue);

                if (rootNode.Maximize && evaluation < bestEvaluation)
                {
                    node = null;
                    continue;
                }

                if (!rootNode.Maximize && evaluation >= bestEvaluation)
                {
                    node = null;
                    continue;
                }

                rootNode[move] = node;

                bestEvaluation = evaluation;

                evaluatedMoves.Add(
                    (move, evaluation)
                );
            }

            List<Move> sortedMoves;
            if (board.CurrentTeamTurn == TeamColor.White)
            {
                sortedMoves = (from moveEvaluation in evaluatedMoves
                               orderby moveEvaluation.Item2 descending
                               select moveEvaluation.Item1).ToList();
            }
            else
            {
                sortedMoves = (from moveEvaluation in evaluatedMoves
                               orderby moveEvaluation.Item2 ascending
                               select moveEvaluation.Item1).ToList();
            }

            // select move
            Move chosenMove = sortedMoves[1];

            // discard sibling moves now that one has been chosen.
            rootNode = rootNode[chosenMove];

            board.PerformMove(chosenMove);
        }
    }
}

/*
            List<(int, Move)> moves = new List<(int, Move)>();
            foreach (var move in board.GetMoves())
            {
                Chessboard rootNode = new Chessboard(board, move);

                moves.Add((MinimaxSearch(rootNode, 2, int.MinValue, int.MaxValue), move));
            }*/

/*if (node.Maximize)
            {
                int maxEvaluation = int.MinValue;

                foreach (var move in node.Chessboard.GetMoves())
                {
                    node[move] = new TreeNode(new Chessboard(node.Chessboard, move));

                    maxEvaluation = Math.Max(maxEvaluation, MinimaxSearch(node[move], depth - 1, alpha, beta));
                    alpha = Math.Max(alpha, maxEvaluation);

                    // a better move was found earlier.
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                node.Value = maxEvaluation;
                return maxEvaluation;
            }
            else
            {
                int minEvaluation = int.MaxValue;

                foreach (var move in node.Chessboard.GetMoves())
                {
                    Chessboard childNode = new Chessboard(node.Chessboard, move);

                    minEvaluation = Math.Min(minEvaluation, MinimaxSearch(new TreeNode(childNode), depth - 1, alpha, beta));
                    beta = Math.Min(beta, minEvaluation);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                node.Value = minEvaluation;
                return minEvaluation;
            }*/