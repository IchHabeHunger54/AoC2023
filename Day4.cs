namespace AoC2023;

public class Day4 : Day
{
    public Day4() : base(4)
    {
    }

    protected override object Part1(string path)
    {
        var result = 0d;
        foreach (var line in Util.ReadFileLines(path))
        {
            var split = line.Split(":")[1].Split("|");
            var left = SplitOnSpace(split[0]);
            var right = SplitOnSpace(split[1]);
            var overlap = GetOverlap(left, right).Count;
            if (overlap > 0)
            {
                result += Math.Pow(2, overlap - 1);
            }
        }
        return result;
    }

    private static IEnumerable<string> SplitOnSpace(string s)
    {
        return s.Split(" ").ToList().Where(e => !string.IsNullOrEmpty(e));
    }

    private static List<T> GetOverlap<T>(IEnumerable<T> list1, IEnumerable<T> list2)
    {
        return list1.Where(list2.Contains).ToList();
    }

    protected override object Part2(string path)
    {
        throw new NotImplementedException();
    }
}