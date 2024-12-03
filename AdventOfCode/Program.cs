namespace DaveClark.AdventOfCode;

internal abstract class Program
{
    private static async Task<int> Main(string[] args)
    {
        var year = DateTime.UtcNow.Year;
        var day = DateTime.UtcNow.Day;

        if (args.Length >= 2 && int.TryParse(args[0], out var inputYear) && int.TryParse(args[1], out var inputDay))
        {
            year = inputYear;
            day = inputDay;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nAdvent of Code {year}, Day {day:D2}.\n");

        var puzzle = PuzzleLoader.LoadPuzzle(year, day);

        if (puzzle == null)
        {
            return -1;
        }

        using var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        await puzzle.SolveAsync(cts.Token);
        
        return 0;
    }
}