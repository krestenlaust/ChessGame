using System;
using System.Text;
using ChessGame;
using ChessGame.Gamemodes;
using ChessGame.Pieces;

namespace ChessCommandline
{
    class Program
    {
        static ConsoleColor currentColor = ConsoleColor.Red;
        static readonly Array consoleColors = Enum.GetValues(typeof(ConsoleColor));

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            Console.WriteLine("Nickname for player 1 (white)? ");
            Player player1 = new Player(Console.ReadLine());

            Console.WriteLine("Nickname for player 2? (black)");
            Player player2 = new Player(Console.ReadLine());

            while (true)
            {
                Game game = new Game(player1, player2, new TurtleChess());
                Board board = game.Board;

                while (true)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    DrawBoard(board);

                    while (true)
                    {
                        Console.WriteLine("Enter move: ");
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
                            Console.WriteLine("Made move");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wrong notation");
                            continue;
                        }
                    }
                    
                }

            }
        }

        static void DrawBoard(Board board)
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
                            boardTile = piece.Color == TeamColor.White ? '♗' : '♝';
                            break;
                        case King _:
                            boardTile = piece.Color == TeamColor.White ? '♔' : '♚';
                            break;
                        case Knight _:
                            boardTile = piece.Color == TeamColor.White ? '♘' : '♞';
                            break;
                        case Pawn _:
                            boardTile = piece.Color == TeamColor.White ? '♙' : '\u265F';
                            break;
                        case Queen _:
                            boardTile = piece.Color == TeamColor.White ? '♕' : '♛';
                            break;
                        case Rook _:
                            boardTile = piece.Color == TeamColor.White ? '♖' : '♜';
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
