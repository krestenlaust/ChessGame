using ChessGame;
using ChessGame.Gamemodes;
using ChessGame.Pieces;
using System;
using System.Text;

namespace ChessCommandline
{
    class Program
    {
        static ConsoleColor currentColor = ConsoleColor.DarkRed;
        static bool showDangersquares;
        static readonly Array consoleColors = Enum.GetValues(typeof(ConsoleColor));

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Nickname for player 1 [default: white]? ");
            string nickname = Console.ReadLine();
            Player player1 = new Player(nickname == string.Empty ? "white" : nickname);
            player1.onTurnStarted += AskForMove;
            Player player2 = new Player(nickname == string.Empty ? "black" : nickname);
            player2.onTurnStarted += AskForMove;

            /*
            Console.WriteLine("Nickname for player 2? [default: black]");
            nickname = Console.ReadLine();
            Player player2 = new Player(nickname == string.Empty ? "black" : nickname);
            player2.onTurnStarted += Player2_onTurnStarted;
            */
            ChessGame.Bots.SimpletronBot bot = new ChessGame.Bots.SimpletronBot();

            while (true)
            {
                Chessboard chessboard = new PawnTestChess().GenerateBoard(player1, player2);
                chessboard.StartGame();

                while (chessboard.isGameInProgress)
                { // (event-based)
                }

                if (chessboard.Winner is null)
                {
                    Console.WriteLine("Draw!");
                }
                else
                {
                    Console.WriteLine($"{chessboard.Winner} has won!");
                }

                Console.ReadLine();
            }
        }

        static void AskForMove(Chessboard board)
        {
            DrawBoard(board);

            while (true)
            {
                Console.Title = $"Material: {board.MaterialSum}";
                Console.WriteLine($"{board.CurrentPlayerTurn.Nickname}'s turn to move: ");
                string move = Console.ReadLine();

                if (move == "c")
                {
                    int i;
                    for (i = 0; i < consoleColors.Length; i++)
                    {
                        if ((ConsoleColor)consoleColors.GetValue(i) == currentColor)
                            break;
                    }

                    currentColor = (ConsoleColor)consoleColors.GetValue((i + 1) % consoleColors.Length);

                    DrawBoard(board);
                    continue;
                }

                if (move == "r")
                {
                    board.UpdateDangerzones();
                    
                    DrawBoard(board);
                    continue;
                }

                if (move == "dangerzone")
                {
                    showDangersquares = !showDangersquares;
                    DrawBoard(board);
                    continue;
                }

                if (board.PerformMove(move, MoveNotation.UCI))
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Incorrect notation/move");
                    continue;
                }
            }
        }

        static void DrawBoard(Chessboard board)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            int i = 0;
            for (int y = 0; y < board.Height; y++)
            {
                for (int x = 0; x < board.Width; x++)
                {
                    char boardTile = ' ';
                    Coordinate tilePosition = new Coordinate(x, y);

                    if (showDangersquares)
                    {
                        int blackDangersquare = board.IsDangerSquare(tilePosition, TeamColor.Black);
                        int whiteDangersquare = board.IsDangerSquare(tilePosition, TeamColor.White);
                        int sumDangersquare = whiteDangersquare - blackDangersquare;
                        boardTile = Math.Abs(sumDangersquare).ToString()[0];

                        if (sumDangersquare > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else if (sumDangersquare < 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            boardTile = ' ';
                        }
                    }
                    

                    Piece piece = board[new Coordinate(x, y)];

                    switch (piece)
                    {
                        case Bishop _:
                            boardTile = piece.Color == TeamColor.Black ? '♗' : '♝';
                            break;
                        case King _:
                            boardTile = piece.Color == TeamColor.Black ? '♔' : '♚';
                            break;
                        case Knight _:
                            boardTile = piece.Color == TeamColor.Black ? '♘' : '♞';
                            break;
                        case Pawn _:
                            boardTile = piece.Color == TeamColor.Black ? '♙' : '\u265F';
                            break;
                        case Queen _:
                            boardTile = piece.Color == TeamColor.Black ? '♕' : '♛';
                            break;
                        case Rook _:
                            boardTile = piece.Color == TeamColor.Black ? '♖' : '♜';
                            break;
                    }

                    if (++i % 2 == 0)
                    {
                        Console.BackgroundColor = currentColor;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    Console.Write(boardTile);
                }
                i++;
                Console.WriteLine();
            }
        }
    }
}
