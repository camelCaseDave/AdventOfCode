namespace DaveClark.AdventOfCode;

public abstract class Puzzle(int? year = null, int? day = null)
{
    private readonly TimeProvider _timeProvider = TimeProvider.System;
    
    private int Year { get; } = year ?? DateTime.UtcNow.Year;

    private int Day { get; } = day ?? DateTime.UtcNow.Day;

    protected abstract Task SolvePuzzleAsync(CancellationToken cancellationToken);

    public async Task<int> SolveAsync(CancellationToken cancellationToken)
    {
        var started = _timeProvider.GetTimestamp();

        try
        {
            await SolvePuzzleAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }

        var completed = _timeProvider.GetTimestamp();
        var elapsed = _timeProvider.GetElapsedTime(started, completed);
        
        Console.WriteLine($"\nTook {elapsed.Microseconds}μs.");
        return 0;
    }

    protected async Task<IEnumerable<string>> ReadInputFileAsync(CancellationToken cancellationToken = default)
    {
        var inputRelativePath = GetInputRelativePath();
        var lines = await File.ReadAllLinesAsync(inputRelativePath, cancellationToken);
        return lines;
    }
    
    protected async Task<string> ReadInputFileAsStringAsync(CancellationToken cancellationToken = default)
    {
        var inputRelativePath = GetInputRelativePath();
        var text = await File.ReadAllTextAsync(inputRelativePath, cancellationToken);
        return text;
    }

    private string GetInputRelativePath()
    {
        var basePath = AppContext.BaseDirectory;
        var relativePath = Path.Combine(basePath, "Puzzles", $"Y{Year}", $"Day{Day:D2}", "input.txt");

        if (!File.Exists(relativePath))
        {
            throw new FileNotFoundException($"Input file not found for Y{Year}, Day{Day:D2}: {relativePath}");
        }

        return relativePath;
    }
}