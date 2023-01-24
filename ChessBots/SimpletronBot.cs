namespace ChessBots
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ChessGame;

    /// <summary>
    /// A very simple bot implementation.
    /// </summary>
    public class SimpletronBot : Player
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpletronBot"/> class with custom player name.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        public SimpletronBot(string name)
            : base(name)
        {
        }

        /// <inheritdoc/>
        public override void TurnStarted(Chessboard board)
        {
            Move move = GenerateMove(board);

            board.PerformMove(move);
        }

        void CheckMovesDerp(NodeState node)
        {
            if (node.Depth == 0)
            {
                node.Board.SimulateMove(FindBestMoves(node.Board, node.Board.CurrentTeamTurn)[0]);
                node.MoveStorage.Add((node.Board.MaterialSum, node.RootMove));
                return;
            }

            foreach (var move in node.Board.GetMoves(node.Board.CurrentTeamTurn))
            {
                Chessboard newBoard = new Chessboard(node.Board);
                newBoard.SimulateMove(move);

                CheckMovesDerp(new NodeState
                {
                    Board = newBoard,
                    Depth = node.Depth - 1,
                    MoveStorage = node.MoveStorage,
                    RootMove = node.RootMove,
                });
            }
        }

        void CheckMovesDeep(Chessboard boardReadonly, int depth, ConcurrentBag<(int Material, Move InitialMove)> moves, Move baseMove = null)
        {
            Chessboard board = new Chessboard(boardReadonly);

            if (depth == 0)
            {
                board.SimulateMove(FindBestMoves(board, boardReadonly.CurrentTeamTurn)[0]);
                moves.Add((board.MaterialSum, baseMove));
                return;
            }

            List<Task> rootMoves = new List<Task>();

            foreach (var move in board.GetMoves(board.CurrentTeamTurn))
            {
                Chessboard newBoard = new Chessboard(board);
                newBoard.SimulateMove(move);

                rootMoves.Add(Task.Run(() => CheckMovesDerp(new NodeState
                {
                    Board = newBoard,
                    Depth = depth - 1,
                    MoveStorage = moves,
                    RootMove = move,
                })));
            }

            Task.WaitAll(rootMoves.ToArray());
        }

        Move GenerateMove(Chessboard board)
        {
            ConcurrentBag<(int, Move)> longerMoves = new ConcurrentBag<(int, Move)>();

            CheckMovesDeep(board, 2, longerMoves);

            List<(int, Move)> sortedMoves = longerMoves.OrderByDescending(material => material.Item1).ToList();

            int newLuckyNumber;
            if (board.CurrentTeamTurn == TeamColor.Black)
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

            if (board.CurrentTeamTurn == TeamColor.Black)
            {
                return sortedMoves[sortedMoves.Count - 1].Item2;
            }
            else
            {
                return sortedMoves[0].Item2;
            }
        }

        List<Move> FindBestMoves(Chessboard board, TeamColor teamColor)
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

        struct NodeState
        {
            public Chessboard Board;
            public int Depth;
            public ConcurrentBag<(int, Move)> MoveStorage;
            public Move RootMove;
        }
    }
}
