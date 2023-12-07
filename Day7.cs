namespace AoC2023;

public class Day7 : Day
{
    public Day7() : base(7)
    {
    }

    protected override object Part1(string path)
    {
        var list = Util.ReadFileLines(path).Select(e => e.Split(" ")).Select(e => (new Hand(e[0]), int.Parse(e[1]))).ToList();
        list.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        var result = 0;
        for (var i = 0; i < list.Count; i++)
        {
            result += (i + 1) * list[i].Item2;
        }
        return result;
    }

    protected override object Part2(string path)
    {
        throw new NotImplementedException();
    }
}

internal class Hand : IComparable
{
    private readonly List<Card> _cards;

    public Hand(string cards)
    {
        _cards = cards.Select(Card.ByLetter).ToList();
    }

    private bool IsFiveOfAKind()
    {
        return _cards.ToList().Distinct().Count() == 1;
    }

    private bool IsFourOfAKind()
    {
        return _cards.ToList().Distinct().Count() == 2 &&
               (_cards.ToList().OrderBy(e => e.Value).Skip(1).Distinct().Count() == 1 ||
                _cards.ToList().OrderBy(e => e.Value).SkipLast(1).Distinct().Count() == 1);
    }

    private bool IsFullHouse()
    {
        return _cards.ToList().Distinct().Count() == 2 &&
               (_cards.ToList().OrderBy(e => e.Value).Skip(2).Distinct().Count() == 1 ||
                _cards.ToList().OrderBy(e => e.Value).SkipLast(2).Distinct().Count() == 1);
    }

    private bool IsThreeOfAKind()
    {
        return _cards.ToList().Distinct().Count() == 3 &&
               (_cards.ToList().OrderBy(e => e.Value).Skip(2).Distinct().Count() == 1 ||
                _cards.ToList().OrderBy(e => e.Value).Skip(1).SkipLast(1).Distinct().Count() == 1 ||
                _cards.ToList().OrderBy(e => e.Value).SkipLast(2).Distinct().Count() == 1);
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
        return _cards.ToList().Distinct().Count() == 4;
    }
    
    private bool IsHighCard()
    {
        return _cards.ToList().Distinct().Count() == 5;
    }

    public int CompareTo(object? obj)
    {
        var that = (Hand) obj;
        if (this.IsFiveOfAKind() && !that.IsFiveOfAKind())
            return 1;
        if (that.IsFiveOfAKind() && !this.IsFiveOfAKind())
            return -1;
        if (this.IsFourOfAKind() && !that.IsFourOfAKind())
            return 1;
        if (that.IsFourOfAKind() && !this.IsFourOfAKind())
            return -1;
        if (this.IsFullHouse() && !that.IsFullHouse())
            return 1;
        if (that.IsFullHouse() && !this.IsFullHouse())
            return -1;
        if (this.IsThreeOfAKind() && !that.IsThreeOfAKind())
            return 1;
        if (that.IsThreeOfAKind() && !this.IsThreeOfAKind())
            return -1;
        if (this.IsTwoPair() && !that.IsTwoPair())
            return 1;
        if (that.IsTwoPair() && !this.IsTwoPair())
            return -1;
        if (this.IsOnePair() && !that.IsOnePair())
            return 1;
        if (that.IsOnePair() && !this.IsOnePair())
            return -1;
        if (this.IsHighCard() && !that.IsHighCard())
            return 1;
        if (that.IsHighCard() && !this.IsHighCard())
            return -1;
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
}

internal class Card : IComparable
{
    public readonly CardValue Value;

    private Card(CardValue value)
    {
        Value = value;
    }

    public static Card ByLetter(char letter)
    {
        return new Card(letter switch
        {
            'A' => CardValue.ACE, 'K' => CardValue.KING, 'Q' => CardValue.QUEEN, 'J' => CardValue.JACK, 'T' => CardValue.TEN, '9' => CardValue.NINE, '8' => CardValue.EIGHT, '7' => CardValue.SEVEN, '6' => CardValue.SIX, '5' => CardValue.FIVE, '4' => CardValue.FOUR, '3' => CardValue.THREE, '2' => CardValue.TWO, _ => throw new ArgumentOutOfRangeException(nameof(letter), letter, null)
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
    ACE = 0, KING = 1, QUEEN = 2, JACK = 3, TEN = 4, NINE = 5, EIGHT = 6, SEVEN = 7, SIX = 8, FIVE = 9, FOUR = 10, THREE = 11, TWO = 12
}