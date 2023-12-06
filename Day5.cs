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

    // Credit: u/Salad-Extension
    protected override object Part2(string path)
    {
        var input = Util.ReadFileLines(path);
        var seeds = input[0].Split(' ').Skip(1).Select(long.Parse).ToList();
        var maps = new List<List<(long from, long to, long adjustment)>>();
        List<(long from, long to, long adjustment)>? currmap = null;
        foreach (var line in input.Skip(2))
        {
            if (line.EndsWith(':'))
            {
                currmap = new List<(long from, long to, long adjustment)>();
                continue;
            }
            if (line.Length == 0 && currmap != null)
            {
                maps.Add(currmap);
                currmap = null;
                continue;
            }
            var nums = line.Split(' ').Select(long.Parse).ToArray();
            currmap!.Add((nums[1], nums[1] + nums[2] - 1, nums[0] - nums[1]));
        }
        if (currmap != null)
        {
            maps.Add(currmap);
        }
        var ranges = new List<(long from, long to)>();
        for (var i = 0; i < seeds.Count; i += 2)
        {
            ranges.Add((from: seeds[i], to: seeds[i] + seeds[i + 1] - 1));
        }
        foreach (var map in maps)
        {
            var orderedmap = map.OrderBy(x => x.from).ToList();
            var newranges = new List<(long from, long to)>();
            foreach (var r in ranges)
            {
                var range = r;
                foreach (var mapping in orderedmap)
                {
                    if (range.from < mapping.from)
                    {
                        newranges.Add((range.from, Math.Min(range.to, mapping.from - 1)));
                        range.from = mapping.from;
                        if (range.from > range.to)
                            break;
                    }
                    if (range.from > mapping.to)
                        continue;
                    newranges.Add((range.from + mapping.adjustment, Math.Min(range.to, mapping.to) + mapping.adjustment));
                    range.from = mapping.to + 1;
                    if (range.from > range.to)
                        break;
                }
                if (range.from <= range.to)
                    newranges.Add(range);
            }
            ranges = newranges;
        }
        return ranges.Min(r => r.from);
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
            return entry.GetFor(i);
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

    public (long, long) Intersect((long, long) l)
    {
        return (Math.Max(_destStart, l.Item1), Math.Min(_destStart + _rangeLength, l.Item2));
    }
}