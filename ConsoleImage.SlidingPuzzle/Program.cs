using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleImage.SlidingPuzzle
{
    class Program
    {
        static void Main(string[] args)
        {

            GameManager manager = new GameManager();
            bool win = manager.Run(args[0]);
            Console.ReadKey();
        }
    }
}
