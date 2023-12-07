namespace AoC2023;

public class Day7 : Day
{
    public Day7() : base(7)
    {
    }

    protected override object Part1(string path)
    {
        return Get(path, false);
    }

    protected override object Part2(string path)
    {
        return Get(path, true);
    }

    private static object Get(string path, bool jokerEnabled)
    {
        var list = Util.ReadFileLines(path).Select(e => e.Split(" ")).Select(e => (new Hand(e[0], jokerEnabled), int.Parse(e[1]))).ToList();
        list.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        var result = 0;
        for (var i = 0; i < list.Count; i++)
        {
            result += (i + 1) * list[i].Item2;
        }
        return result;
    }
}

internal class Hand : IComparable
{
    private readonly List<Card> _cards;

    public Hand(string cards, bool jokerEnabled)
    {
        _cards = cards.Select(e => Card.ByLetter(e, jokerEnabled)).ToList();
    }

    private bool IsFiveOfAKind()
    {
        return _cards.All(e => e.Value == CardValue.JOKER) || ReplaceJokers(_cards).Distinct().Count() == 1;
    }

    private bool IsFourOfAKind()
    {
        var cards = ReplaceJokers(_cards);
        return cards.Distinct().Count() == 2 &&
               (cards.OrderBy(e => e.Value).Skip(1).Distinct().Count() == 1 ||
                cards.OrderBy(e => e.Value).SkipLast(1).Distinct().Count() == 1);
    }

    private bool IsFullHouse()
    {
        var cards = ReplaceJokers(_cards);
        return cards.Distinct().Count() == 2 &&
               (cards.OrderBy(e => e.Value).Skip(2).Distinct().Count() == 1 ||
                cards.OrderBy(e => e.Value).SkipLast(2).Distinct().Count() == 1);
    }

    private bool IsThreeOfAKind()
    {
        var cards = ReplaceJokers(_cards);
        return cards.Distinct().Count() == 3 &&
               (cards.OrderBy(e => e.Value).Skip(2).Distinct().Count() == 1 ||
                cards.OrderBy(e => e.Value).Skip(1).SkipLast(1).Distinct().Count() == 1 ||
                cards.OrderBy(e => e.Value).SkipLast(2).Distinct().Count() == 1);
    }

    private bool IsTwoPair()
    {
        if (_cards.ToList().Distinct().Count() != 3)
            return false;
        var dictionary = new Dictionary<CardValue, int>();
        foreach (var card in _cards)
        {
            if (dictionary.TryGetValue(card.Value, out var value))
            {
                dictionary[card.Value] = value + 1;
            }
            else
            {
                dictionary.Add(card.Value, 1);
            }
        }
        var list = dictionary.OrderBy(e => e.Value).ToList();
        return list[0].Value == 1 && list[1].Value == 2 && list[2].Value == 2;
    }
    
    private bool IsOnePair()
    {
        return ReplaceJokers(_cards).Distinct().Count() == 4;
    }
    
    private bool IsHighCard()
    {
        return ReplaceJokers(_cards).ToList().Distinct().Count() == 5;
    }

    public int CompareTo(object? obj)
    {
        var that = (Hand) obj;
        if (this.IsFiveOfAKind() && that.IsFiveOfAKind())
            return CompareLiteral(that);
        if (this.IsFiveOfAKind() && !that.IsFiveOfAKind())
            return 1;
        if (that.IsFiveOfAKind() && !this.IsFiveOfAKind())
            return -1;
        if (this.IsFourOfAKind() && that.IsFourOfAKind())
            return CompareLiteral(that);
        if (this.IsFourOfAKind() && !that.IsFourOfAKind())
            return 1;
        if (that.IsFourOfAKind() && !this.IsFourOfAKind())
            return -1;
        if (this.IsFullHouse() && that.IsFullHouse())
            return CompareLiteral(that);
        if (this.IsFullHouse() && !that.IsFullHouse())
            return 1;
        if (that.IsFullHouse() && !this.IsFullHouse())
            return -1;
        if (this.IsThreeOfAKind() && that.IsThreeOfAKind())
            return CompareLiteral(that);
        if (this.IsThreeOfAKind() && !that.IsThreeOfAKind())
            return 1;
        if (that.IsThreeOfAKind() && !this.IsThreeOfAKind())
            return -1;
        if (this.IsTwoPair() && that.IsTwoPair())
            return CompareLiteral(that);
        if (this.IsTwoPair() && !that.IsTwoPair())
            return 1;
        if (that.IsTwoPair() && !this.IsTwoPair())
            return -1;
        if (this.IsOnePair() && that.IsOnePair())
            return CompareLiteral(that);
        if (this.IsOnePair() && !that.IsOnePair())
            return 1;
        if (that.IsOnePair() && !this.IsOnePair())
            return -1;
        if (this.IsHighCard() && that.IsHighCard())
            return CompareLiteral(that);
        if (this.IsHighCard() && !that.IsHighCard())
            return 1;
        if (that.IsHighCard() && !this.IsHighCard())
            return -1;
        return CompareLiteral(that);
    }

    private int CompareLiteral(Hand that)
    {
        for (var i = 0; i < 5; i++)
        {
            var compared = this._cards[i].CompareTo(that._cards[i]);
            if (compared != 0)
            {
                return -compared;
            }
        }
        return 0;
    }

    public override string ToString()
    {
        return string.Join("", _cards.Select(e => e.ToString()));
    }

    private static List<Card> ReplaceJokers(List<Card> list)
    {
        if (list.All(e => e.Value == CardValue.JOKER))
            return list.ToList();
        var mostCommon = list.ToList().Where(e => e.Value != CardValue.JOKER).OrderBy(e => e.Value).GroupBy(e => e.Value).OrderByDescending(e => e.Count()).Select(e => e.Key).First();
        return list.ToList().Select(e => e.Value == CardValue.JOKER ? new Card(mostCommon) : e).ToList();
    }
}

internal class Card : IComparable
{
    public readonly CardValue Value;

    public Card(CardValue value)
    {
        Value = value;
    }

    public static Card ByLetter(char letter, bool jokerEnabled)
    {
        return new Card(letter == 'J' ? jokerEnabled ? CardValue.JOKER : CardValue.JACK : letter switch
        {
            'A' => CardValue.ACE, 'K' => CardValue.KING, 'Q' => CardValue.QUEEN, 'T' => CardValue.TEN, '9' => CardValue.NINE, '8' => CardValue.EIGHT, '7' => CardValue.SEVEN, '6' => CardValue.SIX, '5' => CardValue.FIVE, '4' => CardValue.FOUR, '3' => CardValue.THREE, '2' => CardValue.TWO, _ => throw new ArgumentOutOfRangeException(nameof(letter), letter, null)
        });
    }

    public int CompareTo(object? obj)
    {
        return Value.CompareTo(((Card) obj).Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override bool Equals(object? obj)
    {
        return CompareTo(obj) == 0;
    }

    public override int GetHashCode()
    {
        return Value.ToString().GetHashCode();
    }
}

internal enum CardValue
{
    ACE = 0, KING = 1, QUEEN = 2, JACK = 3, TEN = 4, NINE = 5, EIGHT = 6, SEVEN = 7, SIX = 8, FIVE = 9, FOUR = 10, THREE = 11, TWO = 12, JOKER = 13
}