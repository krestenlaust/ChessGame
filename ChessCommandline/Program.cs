using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessGame;

namespace ChessCommandline
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Nickname for player 1? ");
            Player player1 = new Player(Console.ReadLine());

            Console.WriteLine("Nickname for player 2? ");
            Player player2 = new Player(Console.ReadLine());


        }
    }
}
