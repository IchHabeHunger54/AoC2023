namespace AoC2023;

public static class MainClass
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Day 1:");
        Console.WriteLine(Day1.Calibration(@"C:\Users\IHH\data\dev\Other\AoC2023\AoC2023\input\day1.txt"));
    }
}

internal abstract class Day1
{
    public static int Calibration(string path)
    {
        return ReadFileLines(path).Select(Calibrate).Sum();
    }

    private static IEnumerable<string> ReadFileLines(string path)
    {
        var list = new List<string>();
        using TextReader tr = File.OpenText(path);
        while (tr.Peek() != -1)
        {
            list.Add(tr.ReadLine()!);
        }
        return list;
    }

    private static int Calibrate(string input)
    {
        input = input.Replace("one", "o1ne");
        input = input.Replace("two", "t2wo");
        input = input.Replace("three", "t3hree");
        input = input.Replace("four", "f4our");
        input = input.Replace("five", "f5ive");
        input = input.Replace("six", "s6ix");
        input = input.Replace("seven", "s7even");
        input = input.Replace("eight", "e8ight");
        input = input.Replace("nine", "n9ine");
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