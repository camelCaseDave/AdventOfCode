namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day10() : Puzzle(2024, 10)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = await ReadInputFileAsync(cancellationToken);
        var map = new TopoMap(input);
        var total = 0;

        foreach (var trailHead in map.Points.Where(p => p.Z == 0))
        {
            var visited = new HashSet<TopoPoint>();
            total += map.WalkMap(trailHead, visited);
        }

        Console.WriteLine(total);
    }
}

public class TopoMap
{
    public HashSet<TopoPoint> Points { get; set; } = [];

    public TopoMap(IEnumerable<string> input)
    {
        var lines = input.Reverse().ToArray();
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                var value = lines[y][x];
                if (value == '.') continue;
                var height = (int)char.GetNumericValue(value);

                Points.Add(new TopoPoint(x, y, height));
            }
        }
    }
    
    public int WalkMap(TopoPoint current, HashSet<TopoPoint> visited)
    {
        visited.Add(current);

        if (current.Z == 9)
            return 1;

        var count = 0;

        var neighbours = current.GetAdjacentPoints(Points).Where(n => n.Z == current.Z + 1 && !visited.Contains(n));

        foreach (var neighbor in neighbours)
        {
            count += WalkMap(neighbor, visited);
        }

        return count;
    }
}

public class TopoPoint(int x, int y, int z)
{
    private int X { get; } = x;
    private int Y { get; } = y;
    public int Z { get; } = z;

    private readonly (int, int)[] _directions = [(0, -1), (0, 1), (-1, 0), (1, 0)];

    public IEnumerable<TopoPoint> GetAdjacentPoints(IEnumerable<TopoPoint> points)
    {
        foreach (var (dx, dy) in _directions)
        {
            var newX = X + dx;
            var newY = Y + dy;

            var neighbour = points.FirstOrDefault(p => p.X == newX && p.Y == newY);
            if (neighbour != null)
                yield return neighbour;
        }
    }

    public override bool Equals(object obj)
    {
        return obj is TopoPoint point && X == point.X && Y == point.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}