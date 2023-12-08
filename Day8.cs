namespace AoC2023;

public class Day8 : Day
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
        throw new NotImplementedException();
    }
}

internal class Node
{
    public static readonly Dictionary<string, Node> NODES = new();
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