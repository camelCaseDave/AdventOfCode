using System.Collections.Concurrent;

namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day11() : Puzzle(2024, 11)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = (await ReadInputFileAsStringAsync(cancellationToken)).Split(' ');
        const int blinks = 75;
        
        var partitions = Partitioner.Create(input).GetPartitions(Environment.ProcessorCount);
        var tasks = new List<Task<long>>();

        foreach (var partition in partitions)
        {
            tasks.Add(Task.Run(() =>
            {
                var stones = new Dictionary<(string, int), long>();
                var total = 0L;

                while (partition.MoveNext())
                {
                    var stone = partition.Current;
                    total += Blink(stones, stone, blinks);
                }

                return total;
            }, cancellationToken));
        }

        var total = (await Task.WhenAll(tasks)).Sum();
        Console.WriteLine($"Total: {total}");
        
        static long Blink(Dictionary<(string, int), long> stones, string stone, int blinks)
        {
            if (blinks <= 0) return 1;

            blinks--;

            var key = (stone, blinks);

            if (stones.TryGetValue(key, out var cached)) return cached;

            if (stone == "0")
            {
                return stones[key] = Blink(stones, "1", blinks);
            }

            if (stone.Length % 2 == 0)
            {
                var left = stone.Substring(0, stone.Length / 2);
                var right = stone.Substring(stone.Length / 2).TrimStart('0');
                if (right.Length == 0)
                {
                    right = "0";
                }

                return stones[key] = Blink(stones, left, blinks) + Blink(stones, right, blinks);
            }

            var newStone = (2024L * long.Parse(stone)).ToString();
            return stones[key] = Blink(stones, newStone, blinks);
        }
    }
}
