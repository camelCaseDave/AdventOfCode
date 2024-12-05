namespace DaveClark.AdventOfCode.Puzzles.Y2024;

public class Day05() : Puzzle(2024, 5)
{
    protected override async Task SolvePuzzleAsync(CancellationToken cancellationToken)
    {
        var input = (await ReadInputFileAsync(cancellationToken)).ToList();
        var rules = BuildRules(input);
        var updates = input[1177..]
            .Select(line => line.Split(',')
                .Select(int.Parse).ToList())
            .ToList();

        var total = 0;

        // TODO: Remove this unnecessary allocation.
        var ruleBreakers = GetRuleBreakers(rules, updates); 

        foreach (var ruleBreaker in ruleBreakers)
        {
            var sorted = false;
            while (!sorted)
            {
                // TODO: We only need to sort half of the steps in an update to find the middle element.
                foreach (var step in ruleBreaker.ToList())
                {
                    // Rules apply to this step in the update.
                    if (!rules.TryGetValue(step, out var rule)) continue;
                    
                    // Are any rules found earlier in the sequence than the current step, if so step is out of sequence.
                    if (!rule.Intersect(ruleBreaker[..ruleBreaker.IndexOf(step)]).Any()) continue;
                    
                    // Find first occurrence of a rule in ruleBreaker and place step in its correct sequence.
                    ruleBreaker.Remove(step);
                    var first = ruleBreaker.First(x => rule.Contains(x));
                    ruleBreaker.Insert(ruleBreaker.IndexOf(first), step);
                }
                sorted = true;
            }
            
            total += ruleBreaker[ruleBreaker.Count / 2];
        }
        
        Console.WriteLine($"{total}");

        static Dictionary<int, HashSet<int>> BuildRules(List<string> input)
        {
            var rules = new Dictionary<int, HashSet<int>>();
            
            foreach (var line in input[..1176])
            {
                var first = int.Parse(line[..2]);
                var last = int.Parse(line[3..]);

                if (!rules.TryGetValue(first, out var rule))
                {
                    rule = new HashSet<int>(last);
                    rules[first] = rule;
                }

                rule.Add(last);
            }

            return rules;
        }
    }

    private static int PartOne(Dictionary<int, HashSet<int>> rules, List<List<int>> updates)
    {
        var total = 0;
        
        foreach (var update in updates)
        {
            var seen = new List<int>();
            var ruleBreach = false;
            
            foreach (var step in update)
            {
                if (rules.TryGetValue(step, out var rule))
                {
                    if (seen.Intersect(rule).Any())
                    {
                        ruleBreach = true;
                        break;
                    }
                }
                seen.Add(step);
            }

            if (ruleBreach) continue;
            
            total += update[update.Count / 2];
        }

        return total;
    }
    
    private static List<List<int>> GetRuleBreakers(Dictionary<int, HashSet<int>> rules, List<List<int>> updates)
    {
        var ruleBreakers = new List<List<int>>();
        
        foreach (var update in updates)
        {
            var seen = new List<int>();
            
            foreach (var step in update)
            {
                if (rules.TryGetValue(step, out var rule))
                {
                    if (seen.Intersect(rule).Any())
                    {
                        ruleBreakers.Add(update);
                        break;
                    }
                }
                seen.Add(step);
            }
        }

        return ruleBreakers;
    }
}