namespace AoC2023;

public class Day1 : Day
{
    public Day1() : base(1)
    {
    }

    protected override object Part1(string path)
    {
        return Util.ReadFileLines(path).Select(Calibrate).Sum();
    }

    protected override object Part2(string path)
    {
        return Util.ReadFileLines(path).Select(e => e.Replace("one", "o1ne").Replace("two", "t2wo").Replace("three", "t3hree").Replace("four", "f4our").Replace("five", "f5ive").Replace("six", "s6ix").Replace("seven", "s7even").Replace("eight", "e8ight").Replace("nine", "n9ine")).Select(Calibrate).Sum();
    }

    private static int Calibrate(string input)
    {
        var first = -1;
        var last = -1;
        foreach (var c in input.Where(char.IsNumber))
        {
            if (first == -1)
            {
                first = c;
            }
            last = c;
        }
        return (first - '0') * 10 + last - '0';
    }
}