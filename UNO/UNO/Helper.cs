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

        public static void WriteWinners(Player[] _allPlayers)
        {
            Console.Clear();

            // get player sorted by winner rank
            Player[] listedPlayers = SortByWinner(_allPlayers);

            // write winners to console
            for (int i = 0; i < listedPlayers.Length; i++)
            {
                // change Color
                switch (i)
                {
                    case 0: // #1
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case 1: // #2
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    default: // else
                        break;
                }
                // text to console
                Console.WriteLine($"#{i + 1}: {listedPlayers[i].PlayerName}");
            }

            Console.ReadKey();
            Console.ResetColor();
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

        private static Player[] SortByWinner(Player[] _allPlayers)
        {
            Player[] arrayToReturn = new Player[_allPlayers.Length];

            for (int i = 0; i < _allPlayers.Length; i++)
            {
                // get rank
                int rank = _allPlayers[i].winnerRank;
                // write player to array
                if (rank == 0)
                {
                    arrayToReturn[_allPlayers.Length - 1] = _allPlayers[i];
                }
                else
                {
                    arrayToReturn[rank - 1] = _allPlayers[i];
                }
            }

            return arrayToReturn;
        }
    }
}
