using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Card
{
    private static int drawStartHand = 7;
    public static int DrawStartHand { get { return drawStartHand; } set { drawStartHand = value; } }
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
    /// <summary>
    /// Card Value
    /// </summary>
    public enum CardNumber
    {
        ZERO,
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        PLUSTWO,
        SKIP,
        REVERSE,
        WISH,
        PLUSFOUR,

        MAX
    }

    /// <summary>
    /// Color of Card
    /// </summary>
    public enum CardColor { YELLOW, BLUE, GREEN, RED, SPECIAL }
    #endregion


    /// <summary>
    /// Change Color of card (Only works with SPECIAL, see <see cref="CardColor"/>)
    /// </summary>
    /// <param name="_color">new color</param>
    public void ChangeCardColor(CardColor _color)
    {
        if (color == CardColor.SPECIAL)
        {
            color = _color;
        }
    }

    #region static functions
    /// <summary>
    /// Return a Card with <see cref="CardColor"/>.SPECIAL when <see cref="CardNumber"/> = <see cref="CardNumber"/>.PLUSFOUR or .WISH
    /// </summary>
    /// <param name="_number">Number of new Card</param>
    /// <returns>new Card with <see cref="CardColor"/>.SPECIAL, if <see cref="CardNumber"/> not PLUSFOUR or WISH return null</returns>
    public static Card GetSpecial(CardNumber _number)
    {
        if (_number == CardNumber.PLUSFOUR ||
            _number == CardNumber.WISH)
        {
            return new Card(_number, CardColor.SPECIAL);
        }
        else
            return null;
    }

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
        cc = CardColor.YELLOW;
        all.Add(new Card(CardNumber.ZERO, cc));
        for (int i = 0; i < 2; i++)
            for (int y = 1; y < (int)CardNumber.WISH; y++)
                all.Add(new Card((CardNumber)y, cc));

        // Blue
        cc = CardColor.BLUE;
        all.Add(new Card(CardNumber.ZERO, cc));
        for (int i = 0; i < 2; i++)
            for (int y = 1; y < (int)CardNumber.WISH; y++)
                all.Add(new Card((CardNumber)y, cc));

        // Green
        cc = CardColor.GREEN;
        all.Add(new Card(CardNumber.ZERO, cc));
        for (int i = 0; i < 2; i++)
            for (int y = 1; y < (int)CardNumber.WISH; y++)
                all.Add(new Card((CardNumber)y, cc));

        // Red
        cc = CardColor.RED;
        all.Add(new Card(CardNumber.ZERO, cc));
        for (int i = 0; i < 2; i++)
            for (int y = 1; y < (int)CardNumber.WISH; y++)
                all.Add(new Card((CardNumber)y, cc));
                #endregion

        cc = CardColor.SPECIAL;
        for (int i = 0; i < 4; i++)
        {

            all.Add(new Card(CardNumber.WISH, cc));
            all.Add(new Card(CardNumber.PLUSFOUR, cc));
        }

        if (_shuffle)
            all = Shuffle(all);

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

    /// <summary>
    /// Gives each player start cards. See <b><see cref="DrawStartHand"/></b> for Value
    /// </summary>
    /// <param name="_cardDeck">current deck with cards</param>
    /// <param name="_player">all player</param>
    public static void GiveCards(Stack<Card> _cardDeck, params Player[] _player)
    {
        // per Card (every Player get one card, the every player get the second card and so on)
        for (int draw = 0; draw < DrawStartHand; draw++)
        {
            for (int player = 0; player < _player.Length; player++)
            {
                _player[player].CardHand.Add(_cardDeck.Pop());
            }
        }
    }
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
        return color.ToString() + " " + number.ToString();
    }
    #endregion

}

public class Player
{
    /// <summary>Player Number</summary>
    private int playerNumber;
    /// <summary>Cards in his hand</summary>
    private List<Card> cardHand = new List<Card>();
    /// <summary>Player Name</summary>
    public int winnerRank;

    #region Properties
    /// <summary>Player Number</summary>
    public int PlayerNumber { get { return playerNumber; } }
    /// <summary>Cards in his hand</summary>
    public List<Card> CardHand { get { return cardHand; } }
    /// <summary>Player Name</summary>
    public string PlayerName { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// A new Player
    /// </summary>
    /// <param name="_playerNumber">Player Number</param>
    public Player(int _playerNumber)
    {
        playerNumber = _playerNumber;
        PlayerName = "Player " + PlayerNumber;
        winnerRank = 0;
    }
    /// <summary>
    /// A new Player
    /// </summary>
    /// <param name="_playerNumber">Player Number</param>
    /// <param name="_cardHand">Cards on his hand</param>
    public Player(int _playerNumber, List<Card> _cardHand)
    {
        playerNumber = _playerNumber;
        cardHand = _cardHand;
        PlayerName = "Player " + PlayerNumber;
        winnerRank = 0;
    }
    #endregion

    #region override
    public override bool Equals(object obj)
    {
        if (obj.GetType() != this.GetType())
            return false;
        Player other = (Player)obj;
        if (this.PlayerNumber == other.playerNumber)
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
        return PlayerName;
    }
    #endregion

    public string PlayerWithCard()
    {
        string s = $"{PlayerName} ({PlayerNumber}) with {CardHand.Count} Cards";
        return s;

    }
}
