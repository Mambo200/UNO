﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    class Game
    {
        private int nextPlayerRank = 1;
        /// <summary>when reverse card was player</summary>
        bool reverse = false;
        public static Card LastPlayedCard;
        Stack<Card> allCards = new Stack<Card>();
        Player[] players;
        public Player currentPlayer;

        // Win Variables
        private bool inProgress = true;
        private Player hasWon = null;
        private bool drawCard = true;
        private bool skipPlayer = false;

        /// <summary>
        /// Start the Game
        /// </summary>
        public void StartGame()
        {
            // playercount
            int playerCount = 0;
            do
            {
                Console.Write("How many Players? ");
                int.TryParse(Console.ReadLine(), out playerCount);

                if (playerCount <= 0)
                {
                    Console.Clear();
                }
            } while (playerCount <= 0);

            players = new Player[playerCount];

            // generate Player and set name
            for (int i = 0; i < players.Length; i++)
            {
                // new Player
                players[i] = new Player(i + 1);
                // set name
                Helper.SetName(players[i]);
            }
            allCards = Card.GiveDeck(true);
#if DEBUG
            Card.DrawStartHand = 1;
#endif
            // give cards to player
            Card.GiveCards(allCards, players);

            // set last played card
            LastPlayedCard = allCards.Pop();

            // set player 1 as 
            SetCurrentPlayer(0);

            // current player draw card if last card was Special
            DrawFromSpecial(true);


            // actual Game
            DoTurn();

            // Winner text
            Helper.WriteWinners(players);


        }

        /// <summary>
        /// Ingame function
        /// </summary>
        private void DoTurn()
        {
            while (inProgress)
            {
                // Set new Card Deck if current is empty
                if (allCards.Count == 0)
                    allCards = Card.GiveDeck(true);

                // draw cards if last card was a draw card
                DrawFromSpecial();

                // show cards of current player
                ShowCurrentPlayersCards(true);

                // let player choose a card to play
                ChooseCard();

                // check if someone has won
                GameWon();

                if (skipPlayer)
                    NextPlayer();

                // reset skip player
                skipPlayer = false;

                // switch to next player
                NextPlayer();
            }
        }

        /// <summary>
        /// Show other player card count
        /// </summary>
        private void OtherPlayerCardCount()
        {
            string message = "";
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == currentPlayer)
                {
                    message += currentPlayer.PlayerName + " (You):\t" + currentPlayer.CardHand.Count + "\n";
                }
                else
                {
                    message += players[i].PlayerName + ":\t" + players[i].CardHand.Count + "\n";
                }
            }
            Console.WriteLine(message);
        }

        /// <summary>
        /// Show Players cards
        /// </summary>
        private void ShowCurrentPlayersCards(bool _showDrawACard)
        {
            Console.WriteLine("Press any key...");
            Console.Clear();

            string s = currentPlayer.PlayerWithCard() + "\n";
            Console.WriteLine(s);

            int count = -1;

            foreach (Card card in currentPlayer.CardHand)
            {
                count++;
                Console.WriteLine(count + ":\t" + card);
            }

            if (_showDrawACard)
            {
                Console.WriteLine((count+1) + ":\t" + "Draw");

            }

            // show last played card on field
            ShowLastPlayedCard();

            Console.WriteLine();
            // show other players card count
            OtherPlayerCardCount();

        }

        private static void ShowLastPlayedCard()
        {
            Console.WriteLine("\nOn Field: " + LastPlayedCard + "\n\n");
        }

        private void ChooseCard()
        {
            int cardChosen = -1;
            string input = "";
            bool work = false;
            bool drawFromDeck = false;

            // try input as long as there is a correct input
            do
            {
                Console.Write("Choose your Card: ");

                // user input
                input = Console.ReadLine();

                // contert to int
                work = int.TryParse(input, out cardChosen);

                // check if input was a number
                #region input check
                // input was not a number
                if (!work)
                {
                    Console.WriteLine("\n{0} was not a valid number.", input);
                }
                // input was a number
                else
                {
                    // if player choose to draw a card
                    if (cardChosen == currentPlayer.CardHand.Count)
                    {
                        drawFromDeck = true;
                    }
                    // check if number is valid
                    else if (cardChosen < 0 || cardChosen >= currentPlayer.CardHand.Count)
                    {
                        Console.WriteLine("\n{0} was too high / too low.", input);
                        work = false;
                        drawFromDeck = true;
                    }
                }
                #endregion

                // check if cards can be used
                if (!drawFromDeck)
                {
                    Card pickedCard = currentPlayer.CardHand[cardChosen];

                    // check if chosen card is a special card
                    if (pickedCard.Number == Card.CardNumber.PLUSFOUR || pickedCard.Number == Card.CardNumber.WISH)
                    {
                        // check if last played card was special card
                        if (LastPlayedCard.Number != Card.CardNumber.PLUSFOUR && LastPlayedCard.Number != Card.CardNumber.WISH)
                        {
                            // if last card was no special card, chose color of special card
                            ChangeColorSpecialCard(cardChosen);
                        }
                        else
                        {
                            // if last card was already a special card round did not work --> new user input
                            work = false;
                        }
                    }
                    // if number or color are equal
                    else if (pickedCard.Color == LastPlayedCard.Color || pickedCard.Number == LastPlayedCard.Number)
                    {
                        if (pickedCard.Number == Card.CardNumber.PLUSFOUR ||
                            pickedCard.Number == Card.CardNumber.WISH)
                        {
                            work = false;
                        }
                        else
                        {
                            work = true;
                        }
                    }
                    else
                    {
                        work = false;
                    }
                }



                ShowCurrentPlayersCards(true);
            } while (!work);

            
            if (drawFromDeck)
            {
                // player draws a card
                DrawCards();
            }
            else
            {
                // set current last played card
                LastPlayedCard = currentPlayer.CardHand[cardChosen];

                // remove card from player hand
                currentPlayer.CardHand.Remove(LastPlayedCard);

                // skip next player
                if (LastPlayedCard.Number == Card.CardNumber.SKIP)
                {
                    skipPlayer = true;
                }
                if (LastPlayedCard.Number == Card.CardNumber.REVERSE)
                {
                    reverse = !reverse;
                }
                if (LastPlayedCard.Number == Card.CardNumber.PLUSTWO
                    || LastPlayedCard.Number == Card.CardNumber.PLUSFOUR)
                {
                    drawCard = true;
                }
            }

        }

        /// <summary>
        /// Set current player to next player
        /// </summary>
        private void NextPlayer()
        {
            int playerNumber = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == currentPlayer)
                {
                    playerNumber = i;
                    break;
                }
            }

            bool alreadyWon = true;
            // if next player already won do again
            do
            {
                // go in the opposite direction if reverse is true
                if (reverse)
                {
                    playerNumber -= 1;
                    // check if playernumber is below 0
                    if (playerNumber < 0)
                    {
                        playerNumber = players.Length - 1;
                    }
                }
                else
                {
                    playerNumber += 1;
                    // check if playernumber is above maximum
                    if (playerNumber >= players.Length)
                    {
                        playerNumber = 0;
                    }
                }

            // set current player
            SetCurrentPlayer(playerNumber);

                if (currentPlayer.winnerRank == 0)
                    alreadyWon = false;
                else
                    alreadyWon = true;

            } while (alreadyWon);
        }

        private void SetCurrentPlayer(int _playerInArray)
        {
            currentPlayer = players[_playerInArray];
        }

        /// <summary>
        /// Check if last player has zero Cards left
        /// </summary>
        private void GameWon()
        {
            if (currentPlayer.CardHand.Count == 0)
            {
                currentPlayer.winnerRank = nextPlayerRank;
                nextPlayerRank++;
                hasWon = currentPlayer;

            }
            if (nextPlayerRank == players.Length)
            {
                inProgress = false;
            }
        }

        /// <summary>
        /// Change color of special card in Hand
        /// </summary>
        /// <param name="_cardChosen">location of special card</param>
        private void ChangeColorSpecialCard(int _cardChosen)
        {

            bool work = false;

            // choose card
            do
            {
                ShowCurrentPlayersCards(false);

                Console.WriteLine("1:\t" + Card.CardColor.YELLOW);
                Console.WriteLine("2:\t" + Card.CardColor.BLUE);
                Console.WriteLine("3:\t" + Card.CardColor.GREEN);
                Console.WriteLine("4:\t" + Card.CardColor.RED);
                int.TryParse(Console.ReadLine(), out int chosenColor);

                switch (chosenColor)
                {
                    case 1:
                        currentPlayer.CardHand[_cardChosen].ChangeCardColor(Card.CardColor.YELLOW);
                        work = true;
                        break;
                    case 2:
                        currentPlayer.CardHand[_cardChosen].ChangeCardColor(Card.CardColor.BLUE);
                        work = true;
                        break;
                    case 3:
                        currentPlayer.CardHand[_cardChosen].ChangeCardColor(Card.CardColor.GREEN);
                        work = true;
                        break;
                    case 4:
                        currentPlayer.CardHand[_cardChosen].ChangeCardColor(Card.CardColor.RED);
                        work = true;
                        break;
                    default:
                        work = false;
                        break;
                }
            } while (!work);
        }

        /// <summary>
        /// Change color of last played special card 
        /// </summary>
        private void ChangeColorSpecialCard()
        {

            bool work = false;

            // choose card
            do
            {
                ShowCurrentPlayersCards(false);

                Console.WriteLine("1:\t" + Card.CardColor.YELLOW);
                Console.WriteLine("2:\t" + Card.CardColor.BLUE);
                Console.WriteLine("3:\t" + Card.CardColor.GREEN);
                Console.WriteLine("4:\t" + Card.CardColor.RED);
                int.TryParse(Console.ReadLine(), out int chosenColor);

                switch (chosenColor)
                {
                    case 1:
                        LastPlayedCard.ChangeCardColor(Card.CardColor.YELLOW);
                        work = true;
                        break;
                    case 2:
                        LastPlayedCard.ChangeCardColor(Card.CardColor.BLUE);
                        work = true;
                        break;
                    case 3:
                        LastPlayedCard.ChangeCardColor(Card.CardColor.GREEN);
                        work = true;
                        break;
                    case 4:
                        LastPlayedCard.ChangeCardColor(Card.CardColor.RED);
                        work = true;
                        break;
                    default:
                        work = false;
                        break;
                }
            } while (!work);
        }


        /// <summary>
        /// Draw card to current player
        /// </summary>
        /// <param name="_amount">amount of Cards player has to draw</param>
        private void DrawCards(int _amount = 1)
        {
            for (int i = 0; i < _amount; i++)
            {
                if (allCards.Count == 0)
                {
                    allCards = Card.GiveDeck(true);
                }
                currentPlayer.CardHand.Add(allCards.Pop());
            }
        }

        /// <summary>
        /// Draw a card from Deck if last played card was a special card (PlusFour, PlusTwo, Skip)
        /// </summary>
        private void DrawFromSpecial(bool _start = false)
        {
            // if no special card was played last round
            if (drawCard == false)
                return;

            switch (LastPlayedCard.Number)
            {
                case Card.CardNumber.REVERSE:
                    reverse = !reverse;
                    break;
                case Card.CardNumber.WISH:
                    ChangeColorSpecialCard();
                    break;
                case Card.CardNumber.SKIP:
                    NextPlayer();
                    break;
                case Card.CardNumber.PLUSTWO:
                    DrawCards(2);
                    break;
                case Card.CardNumber.PLUSFOUR:
                    DrawCards(4);
                    if (_start)
                    {
                        ChangeColorSpecialCard();
                    }
                    break;
                default:
                    break;
            }

            drawCard = false;
        }
    }
}
