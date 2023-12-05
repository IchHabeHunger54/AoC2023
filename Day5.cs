namespace AoC2023;

public class Day5 : Day
{
    public Day5() : base(5)
    {
    }

    protected override object Part1(string path)
    {
        var lines = Util.ReadFileLines(path);
        var maps = GetMaps(lines);
        return lines[0].Split(" ")[1..].Select(long.Parse).Select(e => EvaluateSeed(e, maps)).Min();
    }

    private static long EvaluateSeed(long seed, IEnumerable<Map> maps)
    {
        return maps.Aggregate(seed, (current, map) => map.Get(current));
    }

    private static IEnumerable<Map> GetMaps(IEnumerable<string> lines)
    {
        var result = new List<Map>();
        Map? current = null;
        foreach (var line in lines)
        {
            try
            {
                var split = line.Split(" ").Select(long.Parse).ToList();
                if (split.Count != 3) continue;
                current ??= new Map();
                current.Add(split[0], split[1], split[2]);
            }
            catch (FormatException)
            {
                if (current != null)
                {
                    result.Add(current);
                    current = null;
                }
            }
        }
        if (current != null)
        {
            result.Add(current);
        }
        return result;
    }

    protected override object Part2(string path)
    {
        throw new NotImplementedException();
    }
}

internal class Map
{
    private readonly List<Entry> _entries = new();

    public void Add(long sourceStart, long destStart, long rangeLength)
    {
        _entries.Add(new Entry(sourceStart, destStart, rangeLength));
    }

    public long Get(long i)
    {
        foreach (var entry in _entries.Where(entry => entry.IsInRange(i)))
        {
            return entry.GetFor(i);
        }
        return i;
    }
}

internal class Entry
{
    private readonly long _sourceStart;
    private readonly long _destStart;
    private readonly long _rangeLength;

    public Entry(long sourceStart, long destStart, long rangeLength)
    {
        _sourceStart = sourceStart;
        _destStart = destStart;
        _rangeLength = rangeLength;
    }

    public bool IsInRange(long i)
    {
        return i >= _destStart && i < _destStart + _rangeLength;
    }

    public long GetFor(long i)
    {
        return IsInRange(i) ? _sourceStart + i - _destStart : i;
    }
}