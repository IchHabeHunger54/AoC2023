using System.Text.RegularExpressions;

namespace AoC2023;

public static class MainClass
{
    private static void Main(string[] args)
    {
        //Console.WriteLine("Day 1:");
        //Console.WriteLine(Day1.Calibration(@"C:\Users\IHH\data\dev\Other\AoC2023\AoC2023\input\day1.txt"));
        //Console.WriteLine("Day 2:");
        //Console.WriteLine(Day2.Parse(@"C:\Users\IHH\data\dev\Other\AoC2023\AoC2023\input\day2.txt", 12, 13, 14));
        //Console.WriteLine(Day2.MinimalPower(@"C:\Users\IHH\data\dev\Other\AoC2023\AoC2023\input\day2.txt"));
        Console.WriteLine("Day 3:");
        Console.WriteLine(Day3.PartNumbers(@"C:\Users\IHH\data\dev\Other\AoC2023\AoC2023\input\day3.txt"));
    }
}

internal abstract class Day3
{
    private const char BLANK = '.';

    public static int PartNumbers(string path)
    {
        var input = Util.ReadFileLines(path);
        return input.Select((l, i) => ParseLine(l).Where(e => IsNumberAdjacentToSymbol(input, i, e.Item2, e.Item3)).Sum(element => element.Item1)).Sum();
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
    
    private static bool IsNumberAdjacentToSymbol(IReadOnlyList<string> input, int line, int startX, int endX)
    {
        var lineLength = input[0].Length;
        if (startX > 0)
        {
            for (var i = -1; i <= 1; i++)
            {
                if (input[Math.Min(Math.Max(0, line + i), input.Count - 1)][startX - 1] != BLANK)
                    return true;
            }
        }
        if (endX < lineLength - 1)
        {
            for (var i = -1; i <= 1; i++)
            {
                if (input[Math.Min(Math.Max(0, line + i), input.Count - 1)][endX + 1] != BLANK)
                    return true;
            }
        }
        if (line > 0)
        {
            for (var i = startX; i <= endX; i++)
            {
                if (input[line - 1][i] != BLANK)
                    return true;
            }
        }
        if (line < input.Count - 1)
        {
            for (var i = startX; i <= endX; i++)
            {
                if (input[line + 1][i] != BLANK)
                    return true;
            }
        }
        return false;
    }
}

internal abstract partial class Day2
{
    private static readonly Regex RED_REGEX = RedRegex();
    private static readonly Regex GREEN_REGEX = GreenRegex();
    private static readonly Regex BLUE_REGEX = BlueRegex();

    public static int MinimalPower(string path)
    {
        return Util.ReadFileLines(path).Select(Minimal).Select(lineResult => lineResult.Item1 * lineResult.Item2 * lineResult.Item3).Sum();
    }

    private static (int, int, int) Minimal(string line)
    {
        var parsed = line.Split(";").Select(ParseOne).ToList();
        var red = parsed.Select(e => e.Item1).Max();
        var green = parsed.Select(e => e.Item2).Max();
        var blue = parsed.Select(e => e.Item3).Max();
        return (red, green, blue);
    }

    public static int Parse(string path, int maxRed, int maxGreen, int maxBlue)
    {
        return Util.ReadFileLines(path).Select(line => line.Split(":")).Where(parts => ParseLine(parts[1], maxRed, maxGreen, maxBlue)).Sum(parts => int.Parse(parts[0]["Game ".Length..]));
    }

    private static bool ParseLine(string line, int maxRed, int maxGreen, int maxBlue)
    {
        return line.Split(";").Select(ParseOne).All(e => IsGamePossible(e.Item1, e.Item2, e.Item3, maxRed, maxGreen, maxBlue));
    }

    private static bool IsGamePossible(int red, int green, int blue, int maxRed, int maxGreen, int maxBlue)
    {
        return red <= maxRed && green <= maxGreen && blue <= maxBlue;
    }

    private static (int, int, int) ParseOne(string input)
    {
        var red = MatchAndToInt(RED_REGEX, input, " red");
        var green = MatchAndToInt(GREEN_REGEX, input, " green");
        var blue = MatchAndToInt(BLUE_REGEX, input, " blue");
        return (red, green, blue);
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
    public static List<string> ReadFileLines(string path)
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