namespace ChessGame.Players
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading;

    /// <summary>
    /// Represents an opponent on Lichess. On turn, makes move as Lichess bot and returns the move of the opponent.
    /// </summary>
    public class LichessBotPlayer : Player
    {
        readonly HttpClient httpClient = new HttpClient();
        readonly StreamReader localGameStream;
        readonly StreamReader localEventStream;
        string gameID;
        string[] lichessMoves;
        bool receivedMove;

        /// <summary>
        /// Initializes a new instance of the <see cref="LichessBotPlayer"/> class, joining a game on Lichess by ID with Lichess bot token.
        /// </summary>
        /// <param name="name">Nickname of the player instance.</param>
        /// <param name="token">Token of Lichess bot.</param>
        /// <param name="gameID">Lichess match ID.</param>
        public LichessBotPlayer(string name, string token, string gameID)
            : base(name)
        {
            this.gameID = gameID;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            localGameStream = new StreamReader(GetGameStream());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LichessBotPlayer"/> class, and seeks a challenge on Lichess.
        /// </summary>
        /// <param name="name">Nickname of the player instance.</param>
        /// <param name="token">Token of Lichess bot.</param>
        /// <param name="localPlayerColor"></param>
        public LichessBotPlayer(string name, string token, TeamColor localPlayerColor)
            : base(name)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            localEventStream = new StreamReader(GetEventStream());
            CreateSeek(localPlayerColor);
            gameID = ReceiveGame();
            localGameStream = new StreamReader(GetGameStream());
        }

        /// <summary>
        /// Opens a get stream that contains events.
        /// </summary>
        /// <returns>Returns event stream.</returns>
        public Stream GetEventStream()
        {
            return httpClient.GetStreamAsync("https://lichess.org/api/stream/event").Result;
        }

        /// <summary>
        /// Opens a get stream for the game.
        /// </summary>
        /// <param name="gameId">The Lichess match ID.</param>
        /// <returns>Returns game stream.</returns>
        public Stream GetGameStream()
        {
            return httpClient.GetStreamAsync($"https://lichess.org/api/bot/game/stream/{gameID}").Result;
        }

        /// <inheritdoc/>
        public override void TurnStarted(Chessboard board)
        {
            if (board.Moves.Count > 0)
            {
                SendMove(board.Moves.Peek());
            }

            board.PerformMove(ReceiveMove(board.CurrentTeamTurn), MoveNotation.UCI);
        }

        /// <summary>
        /// Resigns the game.
        /// </summary>
        public void ResignGame() => httpClient.PostAsync($"https://lichess.org/api/bot/game/{gameID}/resign", null);

        /// <summary>
        /// Sends a move to a game.
        /// </summary>
        /// <param name="move">Move instance.</param>
        public void SendMove(Move move) => SendMove(move.ToString(MoveNotation.UCI));

        /// <summary>
        /// Sends a move in UCI-notation to a game.
        /// </summary>
        /// <param name="move">Move in UCI-notation.</param>
        public void SendMove(string move)
        {
            httpClient.PostAsync($"https://lichess.org/api/bot/game/{gameID}/move/{move}", null);
        }

        void CreateSeek(TeamColor color)
        {
            Dictionary<string, string> postParameters = new Dictionary<string, string>
            {
                ["rated"] = "false",
                ["color"] = Enum.GetName(typeof(TeamColor), color),
                ["time"] = "5",
                ["increment"] = "0",
            };

            httpClient.PostAsync("https://lichess.org/api/board/seek", new FormUrlEncodedContent(postParameters));
        }

        /// <summary>
        /// Waits for a challenge to appear in the event stream.
        /// </summary>
        /// <returns>Lichess match ID.</returns>
        string ReceiveGame()
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

                if (!(gameID is null))
                {
                    return gameID;
                }

                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// Waits for a move to appear in the game stream.
        /// </summary>
        /// <param name="player">The color of the move to wait for.</param>
        /// <returns>Move in UCI-notation.</returns>
        string ReceiveMove(TeamColor player)
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
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// Parses game stream object.
        /// </summary>
        /// <param name="stringObject"></param>
        void ParseGameStreamObject(string stringObject)
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
        void ParseEventStreamObject(string stringObject)
        {
            // {"type":"gameStart","game":{"id":"1lsvP62l"}}
            JObject obj = JObject.Parse(stringObject);

            string msgType = (string)obj["type"];

            switch (msgType)
            {
                case "gameStart":
                    gameID = (string)obj["game"]["id"];
                    break;
                default:
                    break;
            }
        }
    }
}
