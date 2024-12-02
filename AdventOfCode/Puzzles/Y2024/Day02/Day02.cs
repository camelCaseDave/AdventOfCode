namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day02() : Puzzle(2024, 2)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = await ReadInputFileAsync(cancellationToken);
        var safeReports = 0;

        foreach (var report in input)
        {
            var levels = report.Split(' ').Select(int.Parse).ToList();

            if (IsSafe(levels))
            {
                safeReports++;
                continue;
            }

            for (var i = 0; i < levels.Count; i++)
            {
                var retry = levels.Where((_, index) => index != i).ToList(); 
                
                if (IsSafe(retry))
                {
                    safeReports++;
                    break;
                }
            }
        }

        Console.WriteLine(safeReports);
    }

    private static int PartOne(IEnumerable<string> input)
    {
        return input
            .Select(report => report.Split(' ')
                .Select(int.Parse)
                .ToList())
            .Select(levels => IsSafe(levels) ? 1 : 0)
            .Sum();
    }

    private static bool IsSafe(List<int> levels)
    {
        var isSafe = true;
        var initialSign = Math.Sign(levels[0] - levels[1]);

        for (var i = 1; i < levels.Count; i++)
        {
            var diff = levels[i - 1] - levels[i];
            if (Math.Sign(diff) != initialSign || Math.Abs(diff) > 3)
            {
                isSafe = false;
                break;
            }
        }

        return isSafe;
    }
}