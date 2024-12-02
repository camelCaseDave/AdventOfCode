namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day01() : Puzzle(2024, 1)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = await ReadInputFileAsync(cancellationToken);
        var left = new List<int>();
        var right = new Dictionary<int, int>();
        
        foreach (var line in input)
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            left.Add(int.Parse(split[0]));
            
            var rightValue = int.Parse(split[1]);

            if (!right.TryAdd(rightValue, 1))
            {
                right[rightValue]++;
            }
        }
        
        var similarityScore = 0;

        foreach (var v in left)
        {
            if (right.TryGetValue(v, out var value))
            {
                similarityScore += v * value;
            }
        }
            
        Console.WriteLine(similarityScore);
    }

    private int PartOne(IEnumerable<string> input)
    {
        var enumerable = input as string[] ?? input.ToArray();
        var left = new int[enumerable.Length];
        var right = new int[enumerable.Length];
        var index = 0;

        foreach (var line in enumerable)
        {
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            left[index] = int.Parse(split[0]);
            right[index] = int.Parse(split[1]);
            index++;
        }

        Array.Sort(left);
        Array.Sort(right);

        return left.Select((t, i) => Math.Abs(t - right[i])).Sum();
    }
}