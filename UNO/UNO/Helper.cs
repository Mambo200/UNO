using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    public static class Helper
    {
        private static int MaxChars { get { return 8; } }
        public static void SetName(Player _player)
        {
            bool work = false;
            string newName = "";

            do
            {
                do
                {
                    // set new name
                    Console.Clear();
                    Console.Write(_player.PlayerName + ", choose your Name");
                    Console.Write("Name (less or equal " + MaxChars + " characters): ");
                    string input = Console.ReadLine();
                    newName = GetFirstLetters(input, MaxChars);
                } while (string.IsNullOrWhiteSpace(newName));

                // check new name
                Console.WriteLine($"Is {newName} the name you wanted?\nY/N");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Y || key.Key == ConsoleKey.Enter)
                    work = true;
                else
                    work = false;
            } while (!work);

            _player.PlayerName = newName;
        }

        private static string GetFirstLetters(string _word, int _charCount)
        {
            string toReturn = "";
            int count = 0;
            foreach (char c in _word)
            {
                if (count >= 8)
                {
                    break;
                }
                count++;
                toReturn += c;
            }

            return toReturn;
        }
    }
}
