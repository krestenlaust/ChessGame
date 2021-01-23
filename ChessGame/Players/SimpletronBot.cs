using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChessGame.Bots
{
    public class SimpletronBot : Player
    {
        public SimpletronBot(string name) : base(name)
        {
        }

        public override void TurnStarted(Chessboard board)
        {
            Move move = GenerateMove(board);

            board.PerformMove(move);
        }

        private struct NodeState
        {
            public Chessboard Board;
            public int Depth;
            public ConcurrentBag<(int, Move)> MoveStorage;
            public Move RootMove;
        }

        private void CheckMovesDerp(NodeState node)
        {
            if (node.Depth == 0)
            {
                node.Board.SimulateMove(FindBestMoves(node.Board, node.Board.CurrentTurn)[0]);
                node.MoveStorage.Add((node.Board.MaterialSum, node.RootMove));
                return;
            }

            foreach (var move in node.Board.GetMoves(node.Board.CurrentTurn))
            {
                Chessboard newBoard = new Chessboard(node.Board);
                newBoard.SimulateMove(move);

                CheckMovesDerp(new NodeState
                {
                    Board = newBoard,
                    Depth = node.Depth - 1,
                    MoveStorage = node.MoveStorage,
                    RootMove = node.RootMove
                });
            }
        }

        private void CheckMovesDeep(Chessboard boardReadonly, int depth, ConcurrentBag<(int, Move)> moves, Move baseMove = null)
        {
            Chessboard board = new Chessboard(boardReadonly);

            if (depth == 0)
            {
                board.SimulateMove(FindBestMoves(board, boardReadonly.CurrentTurn)[0]);
                moves.Add((board.MaterialSum, baseMove));
                return;
            }

            List<Task> rootMoves = new List<Task>();

            foreach (var move in board.GetMoves(board.CurrentTurn))
            {
                Chessboard newBoard = new Chessboard(board);
                newBoard.SimulateMove(move);

                rootMoves.Add(Task.Run(() => CheckMovesDerp(new NodeState{
                    Board = newBoard,
                    Depth = depth - 1,
                    MoveStorage = moves,
                    RootMove = move
                })));
            }

            Task.WaitAll(rootMoves.ToArray());
        }

        /*
        private void CheckMovesDeep(Chessboard boardReadonly, int depth, ConcurrentBag<(int, Move)> moves, Move baseMove=null)
        {
            Chessboard board = new Chessboard(boardReadonly);

            if (depth == 0)
            {
                board.SimulateMove(FindBestMoves(board, boardReadonly.CurrentTurn)[0]);
                moves.Add((board.MaterialSum, baseMove));
                return;
            }

            foreach (var move in board.GetMoves(board.CurrentTurn))
            {
                Chessboard newBoard = new Chessboard(board);
                newBoard.SimulateMove(move);

                if (baseMove is null)
                {
                    CheckMovesDeep(newBoard, depth - 1, moves, move);
                }
                else
                {
                    CheckMovesDeep(newBoard, depth - 1, moves, baseMove);
                }
            }
        }*/

        private Move GenerateMove(Chessboard board)
        {
            ConcurrentBag<(int, Move)> longerMoves = new ConcurrentBag<(int, Move)>();

            //List<(int, Move)> longerMoves = new List<(int, Move)>();

            //CheckMovesDeep(board, 3, board.CurrentTurn, longerMoves);
            CheckMovesDeep(board, 2, longerMoves);

            List<(int, Move)> sortedMoves = longerMoves.OrderByDescending(material => material.Item1).ToList();


            int newLuckyNumber;
            if (board.CurrentTurn == TeamColor.Black)
            {
                newLuckyNumber = sortedMoves[sortedMoves.Count - 1].Item1;
            }
            else
            {
                newLuckyNumber = sortedMoves[0].Item1;
            }

            List<Move> viableMoves = (from singleMove in sortedMoves
                                     where singleMove.Item1 == newLuckyNumber 
                                     select singleMove.Item2).ToList();

            foreach (var item in viableMoves)
            {
                if (!board.MovedPieces.Contains(item.Moves[0].Piece))
                {
                    return item;
                }
            }

            if (board.CurrentTurn == TeamColor.Black)
            {
                return sortedMoves[sortedMoves.Count - 1].Item2;
            }
            else
            {
                return sortedMoves[0].Item2;
            }
        }

        private List<Move> FindBestMoves(Chessboard board, TeamColor teamColor)
        {
            List<(int, Move)> moves = new List<(int, Move)>();

            foreach (var move in board.GetMoves(teamColor))
            {
                Chessboard boardCheck = new Chessboard(board);
                boardCheck.SimulateMove(move);
                moves.Add((boardCheck.MaterialSum, move));
            }

            if (moves.Count == 0)
            {
                return null;
            }

            List<(int, Move)> sortedMoves = moves.OrderByDescending(material => material.Item1).ToList();

            int luckyNumber;

            if (teamColor == TeamColor.Black)
            {
                luckyNumber = sortedMoves[sortedMoves.Count - 1].Item1;
            }
            else
            {
                luckyNumber = sortedMoves[0].Item1;
            }

            return (from move in sortedMoves where move.Item1 == luckyNumber select move.Item2).ToList();
        }
    }
}
