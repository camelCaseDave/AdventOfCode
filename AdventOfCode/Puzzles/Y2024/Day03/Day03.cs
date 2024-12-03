using System.Text.RegularExpressions;

namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public partial class Day03() : Puzzle(2024, 3)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = await ReadInputFileAsStringAsync(cancellationToken);

        var total = 0;
        var isEnabled = true;

        foreach (var operation in Operations().Matches(input))
        {
            isEnabled = operation.ToString() switch
            {
                "don't()" => false,
                "do()" => true,
                _ => isEnabled
            };

            if (!isEnabled) continue;

            var operands = operation.ToString()?.Split(',');
            if (operands?.Length == 2)
            {
                total += int.Parse(Digits().Replace(operands[0], "")) 
                         * int.Parse(Digits().Replace(operands[1], ""));
            }
        }

        Console.WriteLine(total);
    }

    [GeneratedRegex(@"(mul\(\d{1,3},\d{1,3}\))|(do\(\))|(don't\(\))")]
    private static partial Regex Operations();

    [GeneratedRegex("[^0-9]")]
    private static partial Regex Digits();
}