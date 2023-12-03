namespace AoC2023;

public class Day3 : Day
{
    private const char BLANK = '.';
    private const char GEAR = '*';

    public Day3() : base(3)
    {
    }

    protected override object Part1(string path)
    {
        var input = Util.ReadFileLines(path);
        return input.Select((l, i) => ParseLine(l).Where(e => IsNumberAdjacentToSymbol(input, i, e.Item2, e.Item3, c => c != BLANK) != null).Sum(element => element.Item1)).Sum();
    }

    protected override object Part2(string path)
    {
        var input = Util.ReadFileLines(path);
        var gears = new Dictionary<(int, int), List<int>>();
        for (var i = 0; i < input.Count; i++)
        {
            var line = input[i];
            var parsed = ParseLine(line);
            foreach (var element in parsed)
            {
                var pos = IsNumberAdjacentToSymbol(input, i, element.Item2, element.Item3, c => c == GEAR);
                if (pos == null)
                    continue;
                if (!gears.ContainsKey(pos.Value))
                {
                    gears[pos.Value] = new List<int>();
                }
                gears[pos.Value].Add(element.Item1);
            }
        }
        return gears.Select(entry => entry.Value).Where(value => value.Count == 2).Sum(value => value[0] * value[1]);
    }

    private static IEnumerable<(int, int, int)> ParseLine(string line)
    {
        var list = new List<(int, int, int)>();
        int current = -1, start = -1, end = -1;
        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];
            if (char.IsNumber(c))
            {
                var number = int.Parse(c.ToString());
                current = current == -1 ? number : current * 10 + number;
                start = start == -1 ? i : start;
                end = end == -1 ? i : end + 1;
            }
            else
            {
                if (current == -1)
                    continue;
                list.Add((current, start, end));
                current = start = end = -1;
            }
        }
        if (current != -1 && start != -1 && end != -1)
        {
            list.Add((current, start, end));
        }
        return list;
    }

    private static (int, int)? IsNumberAdjacentToSymbol(IReadOnlyList<string> input, int line, int startX, int endX, Predicate<char> predicate)
    {
        var lineLength = input[0].Length;
        if (startX > 0)
        {
            for (var i = -1; i <= 1; i++)
            {
                var y = Math.Min(Math.Max(0, line + i), input.Count - 1);
                var x = startX - 1;
                if (predicate.Invoke(input[y][x]))
                    return (y, x);
            }
        }
        if (endX < lineLength - 1)
        {
            for (var i = -1; i <= 1; i++)
            {
                var y = Math.Min(Math.Max(0, line + i), input.Count - 1);
                var x = endX + 1;
                if (input[y][x] != BLANK)
                    return (y, x);
            }
        }
        if (line > 0)
        {
            for (var i = startX; i <= endX; i++)
            {
                var y = line - 1;
                if (input[y][i] != BLANK)
                    return (y, i);
            }
        }
        if (line < input.Count - 1)
        {
            for (var i = startX; i <= endX; i++)
            {
                var y = line + 1;
                if (input[y][i] != BLANK)
                    return (y, i);
            }
        }
        return null;
    }
}