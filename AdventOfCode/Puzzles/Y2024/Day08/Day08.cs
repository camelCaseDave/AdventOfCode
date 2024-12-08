
namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day08() : Puzzle(2024, 8)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = await ReadInputFileAsync(cancellationToken);
        var map = new Map(input);
    }

    public int PartOne(Map map, int gridSize)
    {
        var antinodes = new HashSet<Antenna>();

        foreach (var antenna in map.Antennae.Where(a => a.Value != '.' && a.Value != '#'))
        {
            var equalHzAntennae = map.Antennae.Where(a => a != antenna && a.Value == antenna.Value);

            if (!equalHzAntennae.Any()) continue;

            foreach (var partner in equalHzAntennae)
            {
                var dX = 2 * (partner.X - antenna.X);
                var dY = 2 * (partner.Y - antenna.Y);

                var antinode = new Antenna(antenna.X + dX, antenna.Y + dY, '#');

                antinodes.Add(antinode);
            }
        }

        return antinodes
            .Where(a => a.IsAntinode && a.IsInBounds(gridSize))
            .Count();
    }
}

public class Antenna(int x, int y, int value) : Coordinate(x, y), IEquatable<Antenna>
{
    public int Value { get; set; } = value;
    public bool IsAntinode => Value == '#';
    public bool IsInBounds(int gridSize) => X >= 0 && Y >= 0 && X <= gridSize && Y <= gridSize;

    public override bool Equals(object obj) => Equals(obj as Antenna);

    public bool Equals(Antenna? other) => X == other.X && Y == other.Y;

    public override int GetHashCode() => HashCode.Combine(X, Y);
}

public abstract class Coordinate(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
}

public class Map
{
    public HashSet<Antenna> Antennae { get; set; } = [];

    public Map(IEnumerable<string> input)
    {
        int x = 0, y = 0;

        foreach (var line in input.Reverse())
        {
            foreach (var c in line)
            {
                Antennae.Add(new Antenna(x, y, c));
                x = x == input.Count() - 1 ? 0 : x + 1;
            }

            y++;
        }
    }
}
