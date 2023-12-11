using System.Diagnostics;

namespace AoC2023;

public static class MainClass
{
    private static void Main(string[] args)
    {
        new Day1().Run();
        new Day2().Run();
        new Day3().Run();
        new Day4().Run();
        new Day5().Run();
        new Day6().Run();
        new Day7().Run();
        new Day8().Run();
        new Day9().Run();
        new Day10().Run();
    }
}

public abstract class Day
{
    private readonly int _day;

    protected Day(int day)
    {
        _day = day;
    }

    protected abstract object Part1(string path);

    protected abstract object Part2(string path);

    public void Run()
    {
        var path = @"C:\Users\IHH\data\dev\Other\AoC2023\AoC2023\input\day" + _day + ".txt";
        Console.WriteLine("Day " + _day + ":");
        Console.WriteLine("Part 1:");
        var watch = Stopwatch.StartNew();
        Console.WriteLine(Part1(path));
        Console.WriteLine($"Part 1 completed in {watch.ElapsedMilliseconds}ms");
        Console.WriteLine("Part 2:");
        watch = Stopwatch.StartNew();
        Console.WriteLine(Part2(path));
        Console.WriteLine($"Part 2 completed in {watch.ElapsedMilliseconds}ms");
    }
}

public abstract class Util
{
    public static List<string> ReadFileLines(string path)
    {
        return File.ReadAllLines(path).ToList();
    }
}