using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    class Program
    {
        public static bool runGame = true;
        static void Main(string[] args)
        {
            do
            {
                Game g = new Game();
                g.StartGame();
            } while (runGame);
        }
    }
}
