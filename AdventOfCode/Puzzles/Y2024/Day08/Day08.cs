namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day08() : Puzzle(2024, 8)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = await ReadInputFileAsync(cancellationToken);
        var gridSize = input.First().Length;
        var map = new Map(input);
        var antinodes = new HashSet<Antenna>();

        var antennae = map.Antennae
            .Where(a => a.Value != '.' && a.Value != '#')
            .GroupBy(a => a.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var frequency in antennae.Values)
        {
            foreach (var antenna in frequency)
            {
                foreach (var partner in frequency.Where(p => !p.Equals(antenna)))
                {
                    var (dX, dY) = (partner.X - antenna.X, partner.Y - antenna.Y);

                    for (var (x, y) = (partner.X + dX, partner.Y + dY);
                         x >= 0 && y >= 0 && x < gridSize && y < gridSize;
                         x += dX, y += dY)
                    {
                        antinodes.Add(new Antenna(x, y, '#'));
                    }
                }
            }

            antinodes.UnionWith(frequency); // include antennae in sum
        }

        Console.WriteLine($"Part 2: {antinodes.Count}");
    }
}

public class Antenna(int x, int y, int value) : Coordinate(x, y), IEquatable<Antenna>
{
    public int Value { get; } = value;

    public override bool Equals(object obj) => Equals(obj as Antenna);

    public bool Equals(Antenna? other) => X == other?.X && Y == other?.Y;

    public override int GetHashCode() => HashCode.Combine(X, Y);
}

public abstract class Coordinate(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
}

public class Map
{
    public HashSet<Antenna> Antennae { get; } = [];

    public Map(IEnumerable<string> input)
    {
        var lines = input.Reverse().ToArray();
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                Antennae.Add(new Antenna(x, y, lines[y][x]));
            }
        }
    }
}