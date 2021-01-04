using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessGame;
using ChessGame.Gamemodes;

namespace ChessCommandline
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Nickname for player 1 (white)? ");
            Player player1 = new Player(Console.ReadLine());

            Console.WriteLine("Nickname for player 2? (black)");
            Player player2 = new Player(Console.ReadLine());

            while (true)
            {
                Game game = new Game(player1, player2, new TurtleChess());
            }
        }
    }
}
