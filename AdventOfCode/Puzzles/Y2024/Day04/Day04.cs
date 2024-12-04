namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day04() : Puzzle(2024, 4)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = await ReadInputFileAsync(cancellationToken);
        var grid = BuildGrid(input);
        var total = 0;

        foreach (var point in grid.Where(p => p.Value == 'A'))
        {
            var northWest = point.GetNeighbour(grid, MooreNeighbourhood.NorthWest, p => p.Value is 'M' or 'S');
            if (northWest is null) continue;

            var southEast = point.GetNeighbour(grid, MooreNeighbourhood.SouthEast,
                p => p.Value == (northWest.Value == 'M' ? 'S' : 'M'));
            if (southEast is null) continue;

            var northEast = point.GetNeighbour(grid, MooreNeighbourhood.NorthEast, p => p.Value is 'M' or 'S');
            if (northEast is null) continue;

            var southWest = point.GetNeighbour(grid, MooreNeighbourhood.SouthWest, 
                p => p.Value == (northEast.Value == 'M' ? 'S' : 'M'));

            total += southWest is not null ? 1 : 0;
        }

        Console.WriteLine($"Count of X-MAS': {total}.");

        static HashSet<Point> BuildGrid(IEnumerable<string> lines)
        {
            const int gridSize = 140;
            var grid = new HashSet<Point>();
            var x = 0;
            var y = 0;

            foreach (var line in lines.Reverse())
            {
                foreach (var c in line)
                {
                    grid.Add(new Point(x, y, c));
                    x = x == gridSize - 1 ? 0 : x + 1;
                }

                y++;
            }

            return grid;
        }
    }

    public int PartOne(HashSet<Point> grid)
    {
        var total = 0;

        foreach (var point in grid.Where(p => p.Value == 'X'))
        {
            foreach (var direction in Enum.GetValues<MooreNeighbourhood>())
            {
                var neighbouringM = point.GetNeighbour(grid!, direction, p => p.Value == 'M');
                var neighbouringA = neighbouringM?.GetNeighbour(grid!, direction, p => p.Value == 'A');
                var neighbouringS = neighbouringA?.GetNeighbour(grid!, direction, p => p.Value == 'S');

                total += neighbouringS != null ? 1 : 0;
            }
        }

        return total;
    }
}

public class Point(int x, int y, int value)
{
    private int X { get; } = x;
    private int Y { get; } = y;
    public int Value { get; } = value;

    public Point? GetNeighbour(HashSet<Point?> grid, MooreNeighbourhood direction, Func<Point, bool>? predicate = null)
    {
        var neighbour = direction switch
        {
            MooreNeighbourhood.East => grid.FirstOrDefault(p => p?.X == X + 1 && p.Y == Y),
            MooreNeighbourhood.West => grid.FirstOrDefault(p => p?.X == X - 1 && p.Y == Y),
            MooreNeighbourhood.North => grid.FirstOrDefault(p => p?.X == X && p.Y == Y + 1),
            MooreNeighbourhood.South => grid.FirstOrDefault(p => p?.X == X && p.Y == Y - 1),
            MooreNeighbourhood.NorthEast => grid.FirstOrDefault(p => p?.X == X + 1 && p.Y == Y + 1),
            MooreNeighbourhood.NorthWest => grid.FirstOrDefault(p => p?.X == X - 1 && p.Y == Y + 1),
            MooreNeighbourhood.SouthEast => grid.FirstOrDefault(p => p?.X == X + 1 && p.Y == Y - 1),
            MooreNeighbourhood.SouthWest => grid.FirstOrDefault(p => p?.X == X - 1 && p.Y == Y - 1),
            _ => null
        };

        if (neighbour != null && predicate != null && !predicate(neighbour))
        {
            return null;
        }

        return neighbour;
    }
}

public enum MooreNeighbourhood
{
    North,
    East,
    South,
    West,
    NorthEast,
    NorthWest,
    SouthEast,
    SouthWest
}