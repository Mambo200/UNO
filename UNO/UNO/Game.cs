using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    class Game
    {
        Stack<Card> allCards = new Stack<Card>();
        Player[] players = new Player[2];
        public Player currentPlayer;

        /// <summary>
        /// Start the Game
        /// </summary>
        public void StartGame()
        {
            // generate Player
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new Player(i + 1);
            }
            allCards = Card.GiveDeck(true);


            Card.GiveCards(allCards, players);

            // set player 1 as 
            currentPlayer = players[0];

            DoGame();
        }

        /// <summary>
        /// Ingame function
        /// </summary>
        private void DoGame()
        {
            ShowCurrentPlayersCards();
        }

        /// <summary>
        /// Show Players cards
        /// </summary>
        private void ShowCurrentPlayersCards()
        {
            Console.WriteLine("Press any key...");
            Console.ReadKey();
            Console.Clear();

            string s = currentPlayer.ToString() + "\n";
            Console.WriteLine(s);

            foreach (Card card in currentPlayer.CardHand)
            {
                Console.WriteLine(card);
            }
            Console.ReadKey();

        }
    }
}
