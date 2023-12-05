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
                if (split.Count != 3)
                    continue;
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
        var lines = Util.ReadFileLines(path);
        var maps = GetMaps(lines).ToList();
        var seeds = lines[0].Split(" ")[1..].Select(long.Parse).ToList();
        if (seeds.Count % 2 != 0)
            throw new FormatException();
        var ranges = new List<(long, long)>();
        for (var i = 0; i < seeds.Count; i += 2)
        {
            var start = seeds[i];
            ranges.Add((start, start + seeds[i + 1]));
        }
        foreach (var map in maps)
        {
            var newRanges = new List<(long, long)>();
            foreach (var range in ranges)
            {
                newRanges.AddRange(map.Get(range).Distinct());
            }
            ranges = newRanges;
        }
        return ranges.Select(e => e.Item1).Select(l =>
        {
            return maps.Aggregate(l, (current, map) =>
            {
                return map.Get(current);
            });
        }).Min();
    }
}

internal class Map
{
    private readonly List<Entry> _entries = new();

    public void Add(long sourceStart, long destStart, long rangeLength)
    {
        _entries.Add(new Entry(sourceStart, destStart, rangeLength));
        _entries.Sort();
    }

    public long Get(long i)
    {
        foreach (var entry in _entries.Where(entry => entry.IsInRange(i)))
        {
            return entry.GetFor(i);
        }
        return i;
    }

    public IEnumerable<(long, long)> Get((long, long) range)
    {
        var result = new List<(long, long)>();
        foreach (var intersect in _entries.Select(e => e.Intersect(range)).Where(e => e.Item1 < e.Item2))
        {
            if (intersect.Item1 > range.Item1)
            {
                result.Add((range.Item1, intersect.Item1));
            }
            result.Add(intersect);
            if (range.Item2 > intersect.Item2)
            {
                result.Add((intersect.Item2, range.Item2));
            }
        }
        return result.Count == 0 ? new List<(long, long)> { range } : result;
    }
}

internal class Entry : IComparable
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

    public (long, long) Intersect((long, long) l)
    {
        return (Math.Max(_destStart, l.Item1), Math.Min(_destStart + _rangeLength, l.Item2));
    }

    int IComparable.CompareTo(object? o)
    {
        var e = (Entry) o;
        return _destStart.CompareTo(e._destStart);
    }
}