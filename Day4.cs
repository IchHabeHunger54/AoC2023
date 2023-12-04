namespace AoC2023;

public class Day4 : Day
{
    public Day4() : base(4)
    {
    }

    protected override object Part1(string path)
    {
        return Util.ReadFileLines(path).Select(GetScore).Where(e => e > 0).Select(overlap => Math.Pow(2, overlap - 1)).Sum();
    }

    private static int GetScore(string input)
    {
        var split = input.Split(":")[1].Split("|");
        var left = SplitOnSpace(split[0]);
        var right = SplitOnSpace(split[1]);
        return left.Where(right.Contains).Count();
    }

    private static IEnumerable<string> SplitOnSpace(string s)
    {
        return s.Split(" ").ToList().Where(e => !string.IsNullOrEmpty(e));
    }

    protected override object Part2(string path)
    {
        var lines = Util.ReadFileLines(path);
        var cards = new Dictionary<int, int>();
        var result = 0;
        for (var i = 0; i < lines.Count; i++)
        {
            cards.Add(i, 1);
        }
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            var score = GetScore(line);
            for (var j = 1; j <= score; j++)
            {
                var index = i + j;
                if (index > cards.Count) break;
                cards[index] += cards[i];
            }
            result += cards[i];
        }
        return result;
    }
}