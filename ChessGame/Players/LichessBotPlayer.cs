using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Players
{
    /*
    public class LichessBotPlayer : Player
    {
        
        private HttpClient httpClient = new HttpClient();
        private Stream gameStateStream;
        private string gameID;
        private string token;

        public LichessBotPlayer(string name, string token, string gameID) : base(name)
        {
            this.token = token;
            this.gameID = gameID;

        }

        private Stream GetStream()
        {
            return httpClient.GetStreamAsync($"https://lichess.org/api/bot/game/stream/{gameID}").Result;
        }

        public override void TurnStarted(Chessboard board)
        {
            if (board.Moves.Count > 0)
            {
                SendMove(board.Moves.Peek());
            }

            board.PerformMove(RecieveMove(), MoveNotation.UCI);
        }

        public void ResignGame()
        {
            httpClient.PostAsync($"https://lichess.org/api/bot/game/{gameID}/resign", null);
        }

    private void SendMove(Move move)
        {
            
        }
    }
*/
}
