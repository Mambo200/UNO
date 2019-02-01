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
        Reverse,
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
        CardColor cc;
        #region zero to 9, PlusTow, Skip, Reverse
        // Yellow
        cc = CardColor.Yellow;
        all.Add(new Card(CardNumber.Zero, cc));
        for (int i = 0; i < 2; i++)
        {
            all.Add(new Card(CardNumber.One, cc));
            all.Add(new Card(CardNumber.Two, cc));
            all.Add(new Card(CardNumber.Three, cc));
            all.Add(new Card(CardNumber.Four, cc));
            all.Add(new Card(CardNumber.Five, cc));
            all.Add(new Card(CardNumber.Six, cc));
            all.Add(new Card(CardNumber.Seven, cc));
            all.Add(new Card(CardNumber.Eight, cc));
            all.Add(new Card(CardNumber.Nine, cc));
            all.Add(new Card(CardNumber.PlusTwo, cc));
            all.Add(new Card(CardNumber.Skip, cc));
            all.Add(new Card(CardNumber.Reverse, cc));
        }

        // Blue
        cc = CardColor.Blue;
        all.Add(new Card(CardNumber.Zero, cc));
        for (int i = 0; i < 2; i++)
        {
            all.Add(new Card(CardNumber.One, cc));
            all.Add(new Card(CardNumber.Two, cc));
            all.Add(new Card(CardNumber.Three, cc));
            all.Add(new Card(CardNumber.Four, cc));
            all.Add(new Card(CardNumber.Five, cc));
            all.Add(new Card(CardNumber.Six, cc));
            all.Add(new Card(CardNumber.Seven, cc));
            all.Add(new Card(CardNumber.Eight, cc));
            all.Add(new Card(CardNumber.Nine, cc));
            all.Add(new Card(CardNumber.PlusTwo, cc));
            all.Add(new Card(CardNumber.Skip, cc));
            all.Add(new Card(CardNumber.Reverse, cc));
        }

        // Green
        cc = CardColor.Green;
        all.Add(new Card(CardNumber.Zero, cc));
        for (int i = 0; i < 2; i++)
        {
            all.Add(new Card(CardNumber.One, cc));
            all.Add(new Card(CardNumber.Two, cc));
            all.Add(new Card(CardNumber.Three, cc));
            all.Add(new Card(CardNumber.Four, cc));
            all.Add(new Card(CardNumber.Five, cc));
            all.Add(new Card(CardNumber.Six, cc));
            all.Add(new Card(CardNumber.Seven, cc));
            all.Add(new Card(CardNumber.Eight, cc));
            all.Add(new Card(CardNumber.Nine, cc));
            all.Add(new Card(CardNumber.PlusTwo, cc));
            all.Add(new Card(CardNumber.Skip, cc));
            all.Add(new Card(CardNumber.Reverse, cc));
        }

        // Red
        cc = CardColor.Red;
        all.Add(new Card(CardNumber.Zero, cc));
        for (int i = 0; i < 2; i++)
        {
            all.Add(new Card(CardNumber.One, cc));
            all.Add(new Card(CardNumber.Two, cc));
            all.Add(new Card(CardNumber.Three, cc));
            all.Add(new Card(CardNumber.Four, cc));
            all.Add(new Card(CardNumber.Five, cc));
            all.Add(new Card(CardNumber.Six, cc));
            all.Add(new Card(CardNumber.Seven, cc));
            all.Add(new Card(CardNumber.Eight, cc));
            all.Add(new Card(CardNumber.Nine, cc));
            all.Add(new Card(CardNumber.PlusTwo, cc));
            all.Add(new Card(CardNumber.Skip, cc));
            all.Add(new Card(CardNumber.Reverse, cc));
        }
        #endregion

        cc = CardColor.Special;
        for (int i = 0; i < 4; i++)
        {
            all.Add(new Card(CardNumber.Wish, cc));
            all.Add(new Card(CardNumber.PlusFour, cc));
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
