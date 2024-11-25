using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DaveClark.AdventOfCode;

public static class PuzzleLoader
{
    public static Puzzle? LoadPuzzle(int year, int day) 
    {
        var className = $"DaveClark.AdventOfCode.Puzzles.Y{year}.Day{day:D2}";
        var type = Assembly.GetExecutingAssembly().GetType(className);

        if (type == null || !typeof(Puzzle).IsAssignableFrom(type))
        {
            Console.WriteLine($"Solution for Day {day:D2} not found.");
            return null;
        }

        return (Puzzle?)Activator.CreateInstance(type);
    }
}