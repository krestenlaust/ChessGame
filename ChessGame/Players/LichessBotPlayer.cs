using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChessGame.Players
{
    public class LichessBotPlayer : Player
    {
        private readonly HttpClient httpClient = new HttpClient();
        //public readonly HttpClient httpStreamClient = new HttpClient();
        private readonly StreamReader streamReader;
        private string gameId;
        private string[] lichessMoves;

        public LichessBotPlayer(string name, string token, string gameID) : base(name)
        {
            this.gameId = gameID;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            //httpStreamClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            streamReader = new StreamReader(GetStream());
        }

        public Stream GetStream()
        {
            return httpClient.GetStreamAsync($"https://lichess.org/api/bot/game/stream/{gameId}").Result;
        }

        public override void TurnStarted(Chessboard board)
        {
            if (board.Moves.Count > 0)
            {
                SendMove(board.Moves.Peek());
            }

            board.PerformMove(ReceieveMove(board.CurrentTeamTurn), MoveNotation.UCI);
        }

        private string ReceieveMove(TeamColor player)
        {
            // wait for move
            while (true)
            {
                // update latest game info.
                string line;
                while ((line = streamReader.ReadLine()) != string.Empty)
                {
                    ParseStreamObject(line);
                }

                // if divisible by 2, then it's white's turn
                TeamColor waitingFor = lichessMoves.Length % 2 != 0 ? TeamColor.White : TeamColor.Black;

                // should be waiting for this player
                if (waitingFor == player)
                {
                    return lichessMoves[lichessMoves.Length - 1];
                }
                else
                {
                    // wait before checking again.
                    Thread.Sleep(250);
                }
            }
        }

        private void ParseStreamObject(string stringObject)
        {
            JObject obj = JObject.Parse(stringObject);

            string msgType = (string)obj["type"];
            JToken gameState = null;

            switch (msgType)
            {
                case "gameFull":
                    gameState = obj["state"];
                    goto case "gameState";
                case "gameState":
                    if (gameState == null)
                    {
                        gameState = obj.Root;
                    }

                    lichessMoves = ((string)gameState["moves"]).Split(' ');
                    break;
                default:
                    break;
            }
        }

        public void ResignGame()
        {
            httpClient.PostAsync($"https://lichess.org/api/bot/game/{gameId}/resign", null);
        }

        public void SendMove(Move move) => SendMove(move.ToString(MoveNotation.UCI));

        public void SendMove(string move)
        {
            httpClient.PostAsync($"https://lichess.org/api/bot/game/{gameId}/move/{move}", null);
        }
    }
}
