using System;
using System.Text;
using ChessGame;
using ChessGame.Gamemodes;
using ChessGame.Pieces;

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
                Game game = new Game(player1, player2, new OpenWorldChess());
                Chessboard board = game.Board;

                while (true)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    DrawBoard(board);

                    while (true)
                    {
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
            int i = 0;
            for (int y = 0; y <= board.MaxRank; y++)
            {
                for (int x = 0; x <= board.MaxFile; x++)
                {
                    char boardTile = ' ';

                    Piece piece = board[new Coordinate(x, y)];

                    switch (board[new Coordinate(x, y)])
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
