namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day09() : Puzzle(2024, 9)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var diskMap = await ReadInputFileAsStringAsync(cancellationToken);
        var disk = new Disk(diskMap);
        disk.Defrag();

        var checksum = disk.CalculateChecksum();

        Console.WriteLine(checksum);
    }
}

public class Disk
{
    private List<int> Files { get; } = [];
    
    public Disk(string diskMap)
    {
        for (var i = 0; i < diskMap.Length; i++)
        {
            var file = (int)char.GetNumericValue(diskMap[i]);
            var valueToAdd = (i % 2 == 0) ? i / 2 : -1;
            
            Files.AddRange(Enumerable.Repeat(valueToAdd, file));
        }
    }

    public void Defrag()
    {
        for (var id = Files.Max(); id > 0; id--)
        {
            var fileStart = Files.FindIndex(f => f == id);
            var fileEnd = Files.FindLastIndex(f => f == id);
            var fileLength = fileEnd - fileStart + 1;
            
            var firstFreeSpace = Enumerable.Range(0, Files.Count - fileLength + 1)
                .FirstOrDefault(i => Files.Skip(i).Take(fileLength).All(n => n == -1));

            if (firstFreeSpace == 0 || firstFreeSpace > fileStart) continue; // no space available.

            for (var i = 0; i < fileLength; i++)
            {
                Files[firstFreeSpace + i] = id;
                Files[fileStart + i] = -1;
            }
        }
    }
    
    public long CalculateChecksum()
    {
        long checksum = 0;
        
        for (var i = 0; i < Files.Count - 1; i++)
        {
            var file = Files[i];
            if (file == -1) continue;

            checksum += file * i;
        }

        return checksum;
    }
}