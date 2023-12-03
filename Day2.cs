using System.Text.RegularExpressions;

namespace AoC2023;

public partial class Day2 : Day
{
    private static readonly Regex RED_REGEX = RedRegex();
    private static readonly Regex GREEN_REGEX = GreenRegex();
    private static readonly Regex BLUE_REGEX = BlueRegex();

    public Day2() : base(2)
    {
    }

    protected override object Part1(string path)
    {
        return Util.ReadFileLines(path).Select(line => line.Split(":")).Where(parts => ParseLine(parts[1], 12, 13, 14)).Sum(parts => int.Parse(parts[0]["Game ".Length..]));
    }

    protected override object Part2(string path)
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