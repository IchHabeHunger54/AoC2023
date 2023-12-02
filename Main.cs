using System.Text.RegularExpressions;

namespace AoC2023;

public static class MainClass
{
    private static void Main(string[] args)
    {
        //Console.WriteLine("Day 1:");
        //Console.WriteLine(Day1.Calibration(@"C:\Users\IHH\data\dev\Other\AoC2023\AoC2023\input\day1.txt"));
        Console.WriteLine("Day 2:");
        Console.WriteLine(Day2.Parse(@"C:\Users\IHH\data\dev\Other\AoC2023\AoC2023\input\day2.txt", 12, 13, 14));
    }
}

internal abstract partial class Day2
{
    private static readonly Regex RED_REGEX = RedRegex();
    private static readonly Regex GREEN_REGEX = GreenRegex();
    private static readonly Regex BLUE_REGEX = BlueRegex();

    public static int Parse(string path, int maxRed, int maxGreen, int maxBlue)
    {
        return Util.ReadFileLines(path).Select(line => line.Split(":")).Where(parts => ParseLine(parts[1], maxRed, maxGreen, maxBlue)).Sum(parts => int.Parse(parts[0]["Game ".Length..]));
    }

    private static bool ParseLine(string line, int maxRed, int maxGreen, int maxBlue)
    {
        return line.Split(";").Select(ParseOne).All(e => IsGamePossible(e.Item1, e.Item2, e.Item3, maxRed, maxGreen, maxBlue));
    }

    private static (int, int, int) ParseOne(string input)
    {
        var red = MatchAndToInt(RED_REGEX, input, " red");
        var green = MatchAndToInt(GREEN_REGEX, input, " green");
        var blue = MatchAndToInt(BLUE_REGEX, input, " blue");
        return (red, green, blue);
    }
    
    private static bool IsGamePossible(int red, int green, int blue, int maxRed, int maxGreen, int maxBlue)
    {
        return red <= maxRed && green <= maxGreen && blue <= maxBlue;
    }

    private static int MatchAndToInt(Regex regex, string input, string suffix)
    {
        return regex.Matches(input).Select(e => e.Value).Select(e => e[..^suffix.Length]).Select(e => e.Trim()).Select(int.Parse).FirstOrDefault(0);
    }

    [GeneratedRegex("(\\d)+ red", RegexOptions.Compiled)]
    private static partial Regex RedRegex();
    [GeneratedRegex("(\\d)+ green", RegexOptions.Compiled)]
    private static partial Regex GreenRegex();
    [GeneratedRegex("(\\d)+ blue", RegexOptions.Compiled)]
    private static partial Regex BlueRegex();
}

internal abstract class Day1
{
    public static int Calibration(string path)
    {
        return Util.ReadFileLines(path).Select(Calibrate).Sum();
    }

    private static int Calibrate(string input)
    {
        input = input.Replace("one", "o1ne").Replace("two", "t2wo").Replace("three", "t3hree").Replace("four", "f4our").Replace("five", "f5ive").Replace("six", "s6ix").Replace("seven", "s7even").Replace("eight", "e8ight").Replace("nine", "n9ine");
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

internal abstract class Util
{
    public static IEnumerable<string> ReadFileLines(string path)
    {
        var list = new List<string>();
        using TextReader tr = File.OpenText(path);
        while (tr.Peek() != -1)
        {
            list.Add(tr.ReadLine()!);
        }
        return list;
    }
}