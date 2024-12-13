namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day12(): Puzzle(2024, 12)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = await ReadInputFileAsync(cancellationToken);
        var farm = new Farm(input);

        List<HashSet<GardenPlot>> regions = [];
        
        foreach (var plot in farm.Plots)
        {
            if (regions.Any(r => r.Contains(plot))) 
                continue;

            var region = new HashSet<GardenPlot>();
            var toExplore = new Queue<GardenPlot>();

            toExplore.Enqueue(plot);

            while (toExplore.Count > 0)
            {
                var currentPlot = toExplore.Dequeue();

                if (!region.Add(currentPlot))
                    continue;

                var adjacentPlots = currentPlot
                    .GetAdjacentPlots(farm.Plots)
                    .Where(p => p.Value == plot.Value);
                
                // TODO: Part 2 - this won't work.
                // traverse each direction of the perimeter ensuring they're not double counted
                currentPlot.Perimeter = 4 - adjacentPlots.Count();
            
                foreach (var adjacentPlot in adjacentPlots.Where(p => !region.Contains(p)))
                {
                    toExplore.Enqueue(adjacentPlot);
                }
            }

            regions.Add(region);
        }
        
        var total = regions.Sum(region => region.Count * region.Sum(p => p.Perimeter));

        Console.WriteLine(total);
    }
}

public class Farm
{
    public HashSet<GardenPlot> Plots { get; } = [];
    
    public Farm(IEnumerable<string> input)
    {
        var lines = input.Reverse().ToArray();
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                Plots.Add(new GardenPlot(x, y, lines[y][x]));
            }
        }
    }
}

public class GardenPlot(int x, int y, char value)
{
    private int X { get; } = x;
    private int Y { get; } = y;
    public char Value { get; } = value;
    public int Perimeter { get; set; }
    
    private readonly (int, int)[] _directions = [(0, -1), (0, 1), (-1, 0), (1, 0)];

    public IEnumerable<GardenPlot> GetAdjacentPlots(IEnumerable<GardenPlot> plots)
    {
        foreach (var (dx, dy) in _directions)
        {
            var neighbour = plots.FirstOrDefault(p => p.X == X + dx && p.Y == Y + dy);
            if (neighbour != null)
                yield return neighbour;
        }
    }
}
