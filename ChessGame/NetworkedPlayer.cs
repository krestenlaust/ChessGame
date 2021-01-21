using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChessGame
{
    public class NetworkedPlayer : Player
    {
        private readonly NetworkStream stream;

        public NetworkedPlayer(string name, NetworkStream stream) : base(name)
        {
            this.stream = stream;
        }

        public override void TurnStarted(Chessboard board)
        {
            if (board.Moves.Count > 0)
            {
                SendMove(board.Moves.Peek());
            }

            board.PerformMove(RecieveMove(), MoveNotation.UCI);
        }

        private string RecieveMove()
        {
            while (!stream.DataAvailable)
            {
                Thread.Sleep(250);
            }

            byte[] moveAscii = new byte[5];
            try
            {
                stream.Read(moveAscii, 0, moveAscii.Length);
            }
            catch (System.IO.IOException)
            {

            }

            return Encoding.ASCII.GetString(moveAscii).Trim();
        }

        private void SendMove(Move move)
        {
            byte[] moveAscii = Encoding.ASCII.GetBytes(move.ToString(MoveNotation.UCI).PadRight(5));

            try
            {
                stream.Write(moveAscii, 0, moveAscii.Length);
            }
            catch (System.IO.IOException)
            {

            }
        }
    }
}
