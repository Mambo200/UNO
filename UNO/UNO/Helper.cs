using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    public static class Helper
    {
        public static void SetName(Player _player)
        {
            string name = "";
            Console.Clear();
            Console.Write("Name (less or equal 8 characters): ");
            Console.ReadLine();
        }
    }
}
