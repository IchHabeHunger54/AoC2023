namespace AoC2023;

public class Day6 : Day
{
    public Day6() : base(6)
    {
    }

    protected override object Part1(string path)
    {
        var input = Util.ReadFileLines(path);
        var times = input[0].Split(" ").Where(e => e.Length > 0).Skip(1).Select(int.Parse).ToList();
        var distances = input[1].Split(" ").Where(e => e.Length > 0).Skip(1).Select(int.Parse).ToList();
        var results = new List<List<int>>();
        foreach (var time in times)
        {
            var result = new List<int>();
            for (var i = 0; i < time; i++)
            {
                result.Add(i * (time - i));
            }
            results.Add(result);
        }
        for (var i = 0; i < results.Count; i++)
        {
            results[i] = results[i].Where(e => e > distances[i]).ToList();
        }
        return results.Aggregate(1, (current, r) => current * r.Count);
    }

    protected override object Part2(string path)
    {
        var input = Util.ReadFileLines(path);
        var time = long.Parse(input[0].Replace(" ", "").Split(":")[1]);
        var distance = long.Parse(input[1].Replace(" ", "").Split(":")[1]);
        var result = new List<long>();
        for (var i = 0; i < time; i++)
        {
            result.Add(i * (time - i));
        }
        return result.Where(e => e > distance).ToList().Count;
    }
}