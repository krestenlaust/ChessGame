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
        static readonly Array consoleColors = Enum.GetValues(typeof(ConsoleColor));

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Nickname for player 1 [default: white]? ");
            string nickname = Console.ReadLine();
            Player player1 = new Player(nickname == string.Empty ? "white" : nickname);

            Console.WriteLine("Nickname for player 2? [default: black]");
            nickname = Console.ReadLine();
            Player player2 = new Player(nickname == string.Empty ? "black" : nickname);

            while (true)
            {
                Game game = new Game(player1, player2, new TurtleChess());

                while (true)
                {
                    DrawBoard(game.Board);

                    while (true)
                    {
                        Console.Title = $"Material: {game.Board.MaterialSum}";
                        Console.WriteLine($"{game.CurrentPlayerTurn.Nickname}'s turn to move: ");
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

                            break;
                        }

                        if (move == "r")
                        {
                            game.Board.UpdateDangerzones();
                            break;
                        }

                        if (game.MakeMove(move))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Incorrect notation/move");
                            continue;
                        }
                    }

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
                for (int x = 0; x <= board.Width; x++)
                {
                    char boardTile;
                    Coordinate tilePosition = new Coordinate(x, y);

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
                        default:
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
