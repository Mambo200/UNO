﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    class Game
    {
        Stack<Card> allCards = new Stack<Card>();
        public void StartGame()
        {
            allCards = Card.GiveDeck(false);
        }
    }
}
