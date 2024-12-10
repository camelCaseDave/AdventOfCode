namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day10() : Puzzle(2024, 10)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = await ReadInputFileAsync(cancellationToken);
        var map = new TopoMap(input);
        
        var total = map.Points
            .Where(p => p.Z == 0)
            .Sum(trailHead => map.WalkMap(trailHead));

        Console.WriteLine(total);
    }
}

public class TopoMap
{
    public HashSet<TopoPoint> Points { get; } = [];

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
    
    public int WalkMap(TopoPoint currentPoint)
    {
        if (currentPoint.Z == 9)
            return 1;

        var nextPoints = currentPoint
            .GetAdjacentPoints(Points)
            .Where(n => n.Z == currentPoint.Z + 1);

        return nextPoints.Sum(WalkMap);
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
            var neighbour = points.FirstOrDefault(p => p.X == X + dx && p.Y == Y + dy);
            if (neighbour != null)
                yield return neighbour;
        }
    }
}