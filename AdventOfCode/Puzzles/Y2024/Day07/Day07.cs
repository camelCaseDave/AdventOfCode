namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day07() : Puzzle(2024, 7)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = await ReadInputFileAsync(cancellationToken);
        var equations = input.Select(line => new CalibrationEquation(line)).ToList();

        long total = 0;

        foreach (var equation in equations.Where(e => e.CalibrationConstants.Sum() <= e.TestValue))
        {
            if (equation.CanCombineToTestValue())
            {
                total += equation.TestValue;
            }
        }
        
        Console.WriteLine(total);
    }
}

public class CalibrationEquation
{
    public long TestValue { get; }
    public List<int> CalibrationConstants { get; }

    public CalibrationEquation(string line)
    {
        var parts = line.Split(':');
        TestValue = long.Parse(parts[0]);
        CalibrationConstants = parts[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();
    }

    public bool CanCombineToTestValue() => ExploreCalibrationCombinations(0, CalibrationConstants.First());    

    private bool ExploreCalibrationCombinations(int index, long current)
    {
        if (current == TestValue) return true; // we made it.

        if (index == CalibrationConstants.Count - 1) return false; // we've explored all options.

        var nextValue = CalibrationConstants[index + 1]; // try another value.

        if (ExploreCalibrationCombinations(index + 1, current + nextValue)) return true; // '+'
        if (ExploreCalibrationCombinations(index + 1, current * nextValue)) return true; // '*'
        
        var concatenatedValue = long.Parse(current.ToString() + nextValue.ToString()); // '||'
        return ExploreCalibrationCombinations(index + 1, concatenatedValue);
    }
}