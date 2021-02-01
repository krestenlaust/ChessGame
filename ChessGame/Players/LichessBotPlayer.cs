using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace ChessGame.Players
{
    public class LichessBotPlayer : Player
    {
        private readonly HttpClient httpClient = new HttpClient();
        private readonly StreamReader localGameStream;
        private readonly StreamReader localEventStream;
        private string gameId;
        private string[] lichessMoves;
        private bool receivedMove;

        /// <summary>
        /// Tries to join a game by id.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="token"></param>
        /// <param name="gameID"></param>
        public LichessBotPlayer(string name, string token, string gameID) : base(name)
        {
            gameId = gameID;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            localGameStream = new StreamReader(GetGameStream());
        }

        /// <summary>
        /// Seeks a challenge.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="token"></param>
        public LichessBotPlayer(string name, string token, TeamColor localPlayerColor) : base(name)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            localEventStream = new StreamReader(GetEventStream());
            CreateSeek(localPlayerColor);
            gameId = ReceiveGame();
            localGameStream = new StreamReader(GetGameStream());
        }

        /// <summary>
        /// Opens a get stream that contains events.
        /// </summary>
        /// <returns></returns>
        public Stream GetEventStream()
        {
            return httpClient.GetStreamAsync("https://lichess.org/api/stream/event").Result;
        }

        /// <summary>
        /// Opens a get stream for the game.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public Stream GetGameStream()
        {
            return httpClient.GetStreamAsync($"https://lichess.org/api/bot/game/stream/{gameId}").Result;
        }

        /// <summary>
        /// Called by the chessgame when the game is ready to receive lichess player move.
        /// </summary>
        /// <param name="board"></param>
        public override void TurnStarted(Chessboard board)
        {
            if (board.Moves.Count > 0)
            {
                SendMove(board.Moves.Peek(), gameId);
            }

            board.PerformMove(ReceieveMove(board.CurrentTeamTurn), MoveNotation.UCI);
        }

        private void CreateSeek(TeamColor color)
        {
            Dictionary<string, string> postParameters = new Dictionary<string, string>();
            postParameters["rated"] = "false";
            postParameters["color"] = Enum.GetName(typeof(TeamColor), color);
            postParameters["time"] = "5";
            postParameters["increment"] = "0";

            httpClient.PostAsync("https://lichess.org/api/board/seek", new FormUrlEncodedContent(postParameters));
        }

        /// <summary>
        /// Waits for a challenge to appear in the event stream.
        /// </summary>
        /// <returns></returns>
        private string ReceiveGame()
        {
            // wait for move
            while (true)
            {
                // update latest game info.
                string line;
                while ((line = localEventStream.ReadLine()) != string.Empty)
                {
                    ParseEventStreamObject(line);
                }

                if (!(gameId is null))
                {
                    return gameId;
                }
                
                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// Waits for a move to appear in the game stream.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private string ReceieveMove(TeamColor player)
        {
            // wait for move
            while (true)
            {
                // update latest game info.
                string line;
                while ((line = localGameStream.ReadLine()) != string.Empty)
                {
                    ParseGameStreamObject(line);
                }

                // if divisible by 2, then it's white's turn
                TeamColor waitingFor = lichessMoves.Length % 2 != 0 ? TeamColor.White : TeamColor.Black;

                // should be waiting for this player
                if (waitingFor == player && receivedMove)
                {
                    receivedMove = false;
                    return lichessMoves[lichessMoves.Length - 1];
                }
                else
                {
                    // wait before checking again.
                    Thread.Sleep(250);
                }
            }
        }

        /// <summary>
        /// Parses game stream object.
        /// </summary>
        /// <param name="stringObject"></param>
        private void ParseGameStreamObject(string stringObject)
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
                    receivedMove = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Parses game stream object.
        /// </summary>
        /// <param name="stringObject"></param>
        private void ParseEventStreamObject(string stringObject)
        {
            // {"type":"gameStart","game":{"id":"1lsvP62l"}}
            JObject obj = JObject.Parse(stringObject);

            string msgType = (string)obj["type"];

            switch (msgType)
            {
                case "gameStart":
                    gameId = (string)obj["game"]["id"];
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Resigns a game.
        /// </summary>
        /// <param name="gameId"></param>
        public void ResignGame()
        {
            httpClient.PostAsync($"https://lichess.org/api/bot/game/{gameId}/resign", null);
        }

        /// <summary>
        /// Sends a move to a game.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="gameId"></param>
        public void SendMove(Move move, string gameId) => SendMove(move.ToString(MoveNotation.UCI));

        /// <summary>
        /// Sends a move in UCI-notation to a game.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="gameId"></param>
        public void SendMove(string move)
        {
            httpClient.PostAsync($"https://lichess.org/api/bot/game/{gameId}/move/{move}", null);
        }
    }
}
