namespace AoC2023;

public class Day9 : Day
{
    public Day9() : base(9)
    {
    }

    protected override object Part1(string path)
    {
        var input = Util.ReadFileLines(path);
        var result = 0;
        foreach (var lines in input.Select(line => line.Split(" ").Where(e => e.Length > 0).Select(int.Parse).ToList()).Select(numbers => new List<List<int>> { numbers }))
        {
            while (lines.Last().Any(e => e != 0))
            {
                var prev = lines.Last();
                var current = new List<int>();
                for (var i = 0; i < prev.Count - 1; i++)
                {
                    current.Add(prev[i + 1] - prev[i]);
                }
                lines.Add(current);
            }
            lines.Last().Add(0);
            for (var i = lines.Count - 2; i >= 0; i--)
            {
                lines[i].Add(lines[i].Last() + lines[i + 1].Last());
            }
            result += lines[0].Last();
        }
        return result;
    }

    protected override object Part2(string path)
    {
        var input = Util.ReadFileLines(path);
        var result = 0;
        foreach (var lines in input.Select(line => line.Split(" ").Where(e => e.Length > 0).Select(int.Parse).ToList()).Select(numbers => new List<List<int>> { numbers }))
        {
            while (lines.Last().Any(e => e != 0))
            {
                var prev = lines.Last();
                var current = new List<int>();
                for (var i = 0; i < prev.Count - 1; i++)
                {
                    current.Add(prev[i + 1] - prev[i]);
                }
                lines.Add(current);
            }
            lines.Last().Insert(0, 0);
            for (var i = lines.Count - 2; i >= 0; i--)
            {
                lines[i].Insert(0, lines[i][0] - lines[i + 1][0]);
            }
            result += lines[0][0];
        }
        return result;
    }
}