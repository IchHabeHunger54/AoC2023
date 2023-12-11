namespace AoC2023;

public class Day10 : Day
{
    public Day10() : base(10)
    {
    }

    protected override object Part1(string path)
    {
        var map = new Day10Map(Util.ReadFileLines(path));
        var steps = map.Start!.Neighbors;
        if (steps.Count != 2)
            throw new Exception();
        Day10Node? step1 = steps[0], step2 = steps[1];
        int d1 = 1, d2 = 1;
        var visited = new List<Point> { step1.Pos, step2.Pos };
        while (true)
        {
            step1 = step1?.Neighbors.Where(e => !visited.Contains(e.Pos)).FirstOrDefault((Day10Node?) null);
            step2 = step2?.Neighbors.Where(e => !visited.Contains(e.Pos)).FirstOrDefault((Day10Node?) null);
            if (step1 == null && step2 == null)
                break;
            if (step1 != null)
            {
                visited.Add(step1.Pos);
            }
            if (step2 != null)
            {
                visited.Add(step2.Pos);
            }
            d1++;
            d2++;
        }
        return Math.Max(d1, d2);
    }

    protected override object Part2(string path)
    {
        throw new NotImplementedException();
    }
}

internal class Day10Map
{
    public Day10Node? Start;
    private readonly List<Day10Node> _nodes = new();
    private readonly int _width;
    private readonly int _height;

    public Day10Map(IReadOnlyList<string> lines)
    {
        _width = lines[0].Length;
        _height = lines.Count;
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var pos = lines[y][x];
                var node = new Day10Node(x, y, pos);
                _nodes.Add(node);
                if (pos == 'S')
                {
                    Start = node;
                }
            }
        }
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var node = At(x, y);
                if (Equals(node, Start) || node.IsFacingNorth() && y > 0)
                {
                    var other = At(x, y - 1);
                    if (other.IsFacingSouth())
                    {
                        node.AddNeighbor(other);
                    }
                }
                if (Equals(node, Start) || node.IsFacingEast() && x < _width - 1)
                {
                    var other = At(x + 1, y);
                    if (other.IsFacingWest())
                    {
                        node.AddNeighbor(other);
                    }
                }
                if (Equals(node, Start) || node.IsFacingSouth() && y < _height - 1)
                {
                    var other = At(x, y + 1);
                    if (other.IsFacingNorth())
                    {
                        node.AddNeighbor(other);
                    }
                }
                if (Equals(node, Start) || node.IsFacingWest() && x > 0)
                {
                    var other = At(x - 1, y);
                    if (other.IsFacingEast())
                    {
                        node.AddNeighbor(other);
                    }
                }
            }
        }
    }

    public Day10Node At(int x, int y)
    {
        return _nodes[y * _width + x];
    }

    public Day10Node At(Point point)
    {
        return At(point.X, point.Y);
    }
}

internal class Day10Node
{
    public readonly List<Day10Node> Neighbors = new();
    public readonly Point Pos;
    private readonly Shape? _shape;

    public Day10Node(int x, int y, char shape)
    {
        Pos = new Point(x, y);
        _shape = ByChar(shape);
    }

    public void AddNeighbor(Day10Node node)
    {
        Neighbors.Add(node);
    }

    private static Shape? ByChar(char c)
    {
        return c switch
        {
            '|' => Shape.VERTICAL_PIPE, '-' => Shape.HORIZONTAL_PIPE, 'L' => Shape.NORTHEAST, 'J' => Shape.NORTHWEST, '7' => Shape.SOUTHWEST, 'F' => Shape.SOUTHEAST, _ => null
        };
    }

    public override bool Equals(object? obj)
    {
        return this == obj || (obj is Day10Node node && Pos.Equals(node.Pos));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Neighbors, Pos, _shape);
    }

    public bool IsFacingNorth()
    {
        return _shape is Shape.VERTICAL_PIPE or Shape.NORTHWEST or Shape.NORTHEAST;
    }

    public bool IsFacingEast()
    {
        return _shape is Shape.HORIZONTAL_PIPE or Shape.NORTHEAST or Shape.SOUTHEAST;
    }

    public bool IsFacingSouth()
    {
        return _shape is Shape.VERTICAL_PIPE or Shape.SOUTHEAST or Shape.SOUTHWEST;
    }

    public bool IsFacingWest()
    {
        return _shape is Shape.HORIZONTAL_PIPE or Shape.SOUTHWEST or Shape.NORTHWEST;
    }
}

internal class Direction
{
    public static readonly Direction NORTH = new(0, -1);
    public static readonly Direction EAST = new(1, 0);
    public static readonly Direction SOUTH = new(0, 1);
    public static readonly Direction WEST = new(-1, 0);
    public static readonly List<Direction> ALL = new() { NORTH, EAST, SOUTH, WEST };
    private static readonly Direction NONE = new(0, 0);
    private readonly int _x;
    private readonly int _y;

    private Direction(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public Point Offset(Point input)
    {
        return new Point(input.X + _x, input.Y + _y);
    }

    public Direction GetOpposite()
    {
        return this == NORTH ? SOUTH : this == EAST ? WEST : this == SOUTH ? NORTH : this == WEST ? EAST : NONE;
    }
}

internal enum Shape
{
    VERTICAL_PIPE = 0, HORIZONTAL_PIPE = 1, NORTHEAST = 2, NORTHWEST = 3, SOUTHWEST = 4, SOUTHEAST = 5
}

internal readonly record struct Point(int X, int Y);
