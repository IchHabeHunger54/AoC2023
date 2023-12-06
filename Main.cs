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
        Console.WriteLine(Part1(path));
        Console.WriteLine(Part2(path));
    }
}

public abstract class Util
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