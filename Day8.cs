using System.Text.RegularExpressions;

namespace AoC2023;

public partial class Day8 : Day
{
    public Day8() : base(8)
    {
    }

    protected override object Part1(string path)
    {
        var lines = Util.ReadFileLines(path);
        var directions = lines[0];
        foreach (var line in lines.Skip(2))
        {
            var node = line.Replace(" ", "").Replace("(", "").Replace(")", "").Split("=");
            var lr = node[1].Split(",");
            new Node(node[0], lr[0], lr[1]);
        }
        var result = 1;
        var steps = 0;
        var nodes = Node.Get("AAA");
        while (true)
        {
            nodes = directions[steps] switch
            {
                'L' => nodes.GetLeft(), 'R' => nodes.GetRight(), _ => nodes
            };
            if (nodes.Name == "ZZZ")
                break;
            result++;
            steps++;
            steps %= directions.Length;
        }
        return result;
    }

    protected override object Part2(string path)
    {
        var lines = Util.ReadFileLines(path).ToArray();
        var moves = lines[0].ToCharArray();
        var nodes = lines[2..].Select(line => MyRegex().Match(line)).Select(m => (m.Groups[1].Value, (m.Groups[2].Value, m.Groups[3].Value))).ToDictionary(e => e.Item1, e => e.Item2);
        return nodes.Keys.Where(k => k[2] == 'A').Select(p => PathLength(moves, nodes, p, "Z")).Aggregate(Lcm);
    }

    private static long PathLength(IReadOnlyList<char> moves, IReadOnlyDictionary<string, (string Left, string Right)> nodes, string node, string ending)
    {
        long count = 0;
        var curr = 0;
        while (!node.EndsWith(ending))
        {
            node = moves[curr] == 'L' ? nodes[node].Left : nodes[node].Right;
            curr = (curr + 1) % moves.Count;
            count++;
        }
        return count;
    }

    private static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            var remainder = a % b;
            a = b;
            b = remainder;
        }
        return a;
    }

    private static long Lcm(long a, long b) => a * b / Gcd(a, b);

    [GeneratedRegex(@"(\w+) = \((\w+), (\w+)\)")]
    private static partial Regex MyRegex();
}

internal class Node
{
    private static readonly Dictionary<string, Node> NODES = new();
    public readonly string Name;
    private readonly string _left;
    private readonly string _right;

    public Node(string name, string left, string right)
    {
        Name = name;
        _left = left;
        _right = right;
        NODES.Add(name, this);
    }

    public static Node Get(string name)
    {
        return NODES[name];
    }

    public Node GetLeft()
    {
        return NODES[_left];
    }

    public Node GetRight()
    {
        return NODES[_right];
    }
}