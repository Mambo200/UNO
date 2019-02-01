using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Card
{
    private static Random random = new Random();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="_number">Number of Card</param>
    /// <param name="_color">Color of Card</param>
    public Card(CardNumber _number, CardColor _color)
    {
        number = _number;
        color = _color;
    }

    /// <summary>Get Number of current Card</summary>
    public CardNumber Number { get { return number; } }
    /// <summary>Get Color of current Card</summary>
    public CardColor Color { get { return color; } }

    private CardNumber number;
    private CardColor color;



    #region enum
    public enum CardNumber
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        PlusTwo,
        Skip,
        Turn,
        Wish,
        PlusFour
    }

    public enum CardColor { Yellow, Blue, Green, Red, Special }
    #endregion

    #region override function
    public override bool Equals(object obj)
    {
        if (this.GetType() != obj.GetType())
            return false;

        Card other = (Card)obj;

        if (this.color == other.color && this.number == other.number)
            return true;
        else
            return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return number.ToString() + ", " + color.ToString();
    }
    #endregion

    /// <summary>
    /// Creates a complete Deck
    /// </summary>
    /// <param name="_shuffle">true: shuffle deck</param>
    /// <returns>a complete deck</returns>
    public static Stack<Card> GiveDeck(bool _shuffle)
    {
        List<Card> all = new List<Card>();

        // normal numbers
        for (CardColor col = 0; col <= CardColor.Special; col++)
        {
            for (CardNumber c = 0; c <= CardNumber.Wish; c++)
            {
                Card card = new Card(c, col);
                all.Add(card);
            }
        }

        // special Cards
        CardColor color = CardColor.Special;

        for (CardNumber num = CardNumber.Wish; num < CardNumber.PlusFour; num++)
        {
            for (int i = 0; i < 4; i++)
            {
                all.Add(new Card(num, color));
            }
        }

        if (_shuffle)
        {
            all = Shuffle(all);
        }

        return ToStack(all);
    }

    /// <summary>
    /// Shuffle Cards (Converts list to array and back to list)
    /// </summary>
    /// <param name="_cards">deck of cards</param>
    /// <returns>shuffled deck</returns>
    private static List<Card> Shuffle(List<Card> _cards)
    {
        Card[] cardArray = _cards.ToArray();

        for (int i = 0; i < cardArray.Length; i++)
        {
            SwapArrayPlace(
                cardArray,
                random.Next(cardArray.Length),
                random.Next(cardArray.Length)
                );
        }

        return cardArray.ToList();
    }

    /// <summary>
    /// Swap two positions from an array
    /// </summary>
    /// <param name="_array">array to swap</param>
    /// <param name="_firstArrayLocation">first position to swap</param>
    /// <param name="_secondArrayLocation">second position to swap</param>
    /// <returns>new swapped array. returns null array when swap didnt work (Out of Range Exception)</returns>
    public static void SwapArrayPlace(Array _array, int _firstArrayLocation, int _secondArrayLocation)
    {
        if (_firstArrayLocation < 0 || _firstArrayLocation >= _array.GetLength(0))
        {
            ArgumentOutOfRangeException oor = new ArgumentOutOfRangeException
                (
                "_firstArrayLocation",
                _firstArrayLocation.ToString() + " was too high or too low. Array lengt is " + _array.Length
                );
            throw oor;
        }
        else if (_secondArrayLocation < 0 || _secondArrayLocation >= _array.Length)
        {
            ArgumentOutOfRangeException oor = new ArgumentOutOfRangeException
                (
                "_firstArrayLocation",
                _secondArrayLocation.ToString() + " was too high or too low. Array lengt is " + _array.Length
                );
            throw oor;
        }
        var p1 = _array.GetValue(_firstArrayLocation);
        var p2 = _array.GetValue(_secondArrayLocation);

        _array.SetValue(p2, _firstArrayLocation);
        _array.SetValue(p1, _secondArrayLocation);
    }


    private static Stack<Card> ToStack(List<Card> _cards)
    {
        Stack<Card> toReturn = new Stack<Card>();

        foreach (Card c in _cards)
        {
            toReturn.Push(c);
        }
        return toReturn;
    }
}
