namespace ChessGame.Players
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents an opponent on Lichess. On turn, makes move as Lichess bot and returns the move of the opponent.
    /// </summary>
    public class LichessBotPlayer : Player
    {
        readonly HttpClient httpClient = new HttpClient();
        readonly StreamReader localGameStream;
        readonly StreamReader localEventStream;
        string gameId;
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
            this.gameId = gameID;
            this.httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            this.localGameStream = new StreamReader(this.GetGameStream());
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
            this.httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            this.localEventStream = new StreamReader(this.GetEventStream());
            this.CreateSeek(localPlayerColor);
            this.gameId = this.ReceiveGame();
            this.localGameStream = new StreamReader(this.GetGameStream());
        }

        /// <summary>
        /// Opens a get stream that contains events.
        /// </summary>
        /// <returns>Returns event stream.</returns>
        public Stream GetEventStream()
        {
            return this.httpClient.GetStreamAsync("https://lichess.org/api/stream/event").Result;
        }

        /// <summary>
        /// Opens a get stream for the game.
        /// </summary>
        /// <param name="gameId">The Lichess match ID.</param>
        /// <returns>Returns game stream.</returns>
        public Stream GetGameStream()
        {
            return this.httpClient.GetStreamAsync($"https://lichess.org/api/bot/game/stream/{this.gameId}").Result;
        }

        /// <inheritdoc/>
        public override void TurnStarted(Chessboard board)
        {
            if (board.Moves.Count > 0)
            {
                this.SendMove(board.Moves.Peek());
            }

            board.PerformMove(this.ReceiveMove(board.CurrentTeamTurn), MoveNotation.UCI);
        }

        /// <summary>
        /// Resigns the game.
        /// </summary>
        public void ResignGame() => this.httpClient.PostAsync($"https://lichess.org/api/bot/game/{this.gameId}/resign", null);

        /// <summary>
        /// Sends a move to a game.
        /// </summary>
        /// <param name="move">Move instance.</param>
        public void SendMove(Move move) => this.SendMove(move.ToString(MoveNotation.UCI));

        /// <summary>
        /// Sends a move in UCI-notation to a game.
        /// </summary>
        /// <param name="move">Move in UCI-notation.</param>
        public void SendMove(string move)
        {
            this.httpClient.PostAsync($"https://lichess.org/api/bot/game/{this.gameId}/move/{move}", null);
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

            this.httpClient.PostAsync("https://lichess.org/api/board/seek", new FormUrlEncodedContent(postParameters));
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
                while ((line = this.localEventStream.ReadLine()) != string.Empty)
                {
                    this.ParseEventStreamObject(line);
                }

                if (!(this.gameId is null))
                {
                    return this.gameId;
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
                while ((line = this.localGameStream.ReadLine()) != string.Empty)
                {
                    this.ParseGameStreamObject(line);
                }

                // if divisible by 2, then it's white's turn
                TeamColor waitingFor = this.lichessMoves.Length % 2 != 0 ? TeamColor.White : TeamColor.Black;

                // should be waiting for this player
                if (waitingFor == player && this.receivedMove)
                {
                    this.receivedMove = false;
                    return this.lichessMoves[this.lichessMoves.Length - 1];
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

                    this.lichessMoves = ((string)gameState["moves"]).Split(' ');
                    this.receivedMove = true;
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
                    this.gameId = (string)obj["game"]["id"];
                    break;
                default:
                    break;
            }
        }
    }
}
