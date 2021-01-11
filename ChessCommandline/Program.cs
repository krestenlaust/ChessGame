using System;
using System.Text;
using ChessGame;
using ChessGame.Gamemodes;
using ChessGame.Pieces;

namespace ChessCommandline
{
    class Program
    {
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

                dirty_makemove:
                    Console.WriteLine("Enter move: ");
                    string move = Console.ReadLine();

                    if (game.MakeMove(move))
                    {
                        Console.WriteLine("Made move");
                    }
                    else
                    {
                        Console.WriteLine("Wrong notation");
                        goto dirty_makemove;
                    }
                }

            }
        }

        static void DrawBoard(Board board)
        {
            for (int y = 0; y <= board.MaxRank; y++)
            {
                StringBuilder sb = new StringBuilder();

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

                    sb.Append(boardTile);
                }

                Console.WriteLine(sb);
            }
        }
    }
}
