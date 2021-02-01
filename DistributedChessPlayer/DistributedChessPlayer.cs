using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChessGame;

namespace DistributedChessPlayer
{
    public class DistributedChessPlayer : Player
    {
        private class ComputerClient
        {
            private readonly TcpClient client;

            public bool isAlive
            {
                get
                {
                    return client?.Connected == true;
                }
            }

            public ComputerClient(TcpClient tcpClient)
            {
                client = tcpClient;
            }

            public bool GetStream(out NetworkStream stream)
            {
                stream = null;

                if (client?.Connected != true)
                {
                    return false;
                }

                stream = client.GetStream();
                return true;
            }
        }

        private Queue<TcpClient> availableClients = new Queue<TcpClient>();
        private TcpListener listener;

        public DistributedChessPlayer(string name) : base(name)
        {
            listener = new TcpListener(IPAddress.Any, 5050);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), null);
        }

        private void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            TcpClient client = listener.EndAcceptTcpClient(ar);
            listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), null);

            availableClients.Enqueue(client);
        }

        /// <summary>
        /// Serializes a chessboard by listing all moves made.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private byte[] CreatePacket(Chessboard board, int depth)
        {
            // initialize stringbuilder with 5 chars per move (UCI-notation)
            StringBuilder sb = new StringBuilder(board.Moves.Count * 5);

            // reverses stack and generates PGN string from it.
            Stack<Move> moves = new Stack<Move>(board.Moves);

            while (moves.Count > 0)
            {
                sb.Append(moves.Pop().ToString(MoveNotation.UCI));
                sb.Append(' ');
            }

            sb.Append(depth);

            string movesString = sb.ToString();
            byte[] packet = new byte[sizeof(int) + movesString.Length];

            BitConverter.GetBytes(movesString.Length).CopyTo(packet, 0);
            Encoding.ASCII.GetBytes(movesString).CopyTo(packet, sizeof(int));

            return packet;
        }

        /// <summary>
        /// Returns a client without a job.
        /// </summary>
        /// <returns></returns>
        private TcpClient GetClient()
        {
            while (true)
            {
                if (availableClients.Count == 0)
                {
                    Thread.Sleep(200);
                    continue;
                }

                TcpClient client = availableClients.Dequeue();
                
                if (client?.Connected != true)
                {
                    Thread.Sleep(200);
                    continue;
                }
   
                // Mark as used — this early to prevent race-conditions
                return client;
            }
        }

        private float RemotelyEvaluatePosition(Chessboard node, TcpClient client, int depth)
        {
            NetworkStream stream = client.GetStream();

            // Construct request
            byte[] request = CreatePacket(node, depth);
            stream.Write(request, 0, request.Length);

            // blocks till answer is recieved
            byte[] response = new byte[sizeof(float)];
            stream.Read(response, 0, sizeof(float));

            availableClients.Enqueue(client);

            return BitConverter.ToSingle(response, 0);
        }

        public override void TurnStarted(Chessboard board)
        {
            List<Task> moveTasks = new List<Task>();

            List<(float, Move)> moves = new List<(float, Move)>();
            List<Move> availableMoves = board.GetMoves().ToList();
            ChessBots.SkakinatorLogic logic = new ChessBots.SkakinatorLogic();

            foreach (var move in availableMoves)
            {
                Chessboard rootNode = new Chessboard(board, move);

                TcpClient client = GetClient();

                moveTasks.Add(
                    Task.Run(() =>
                    {
                        float evaluation = RemotelyEvaluatePosition(rootNode, client, 2);
                        float evaluation2 = logic.MinimaxSearch(rootNode, 2, float.MinValue, float.MaxValue);
                        moves.Add((evaluation, move));
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

            board.PerformMove(sortedMoves[0]);
        }
    }
}
