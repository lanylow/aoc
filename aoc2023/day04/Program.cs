namespace day04;

public static class Program
{
    private static void Main()
    {
        var input = ParseInput().ToList();
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static int SolvePartOne(IEnumerable<int> input) => input.Select(x => 1 << x >> 1).Sum();

    private static int SolvePartTwo(List<int> input)
    {
        var cards = Enumerable.Repeat(1, input.Count).ToArray();
        var i = 0;

        foreach (var card in input)
        {
            for (var j = 1; j <= card; j++)
            {
                if (i + j >= cards.Length)
                {
                    break;
                }

                cards[i + j] += cards[i];
            }

            i++;
        }

        return cards.Sum();
    }
    
    private static IEnumerable<int> ParseInput() => 
        File.ReadAllLines("input.txt")
            .Select(x => x[(x.IndexOf(':') + 2)..].Split('|'))
            .Select(x => x[0].Trim().Replace("  ", " ").Split(' ').ToHashSet().Intersect(x[1].Trim().Replace("  ", " ").Split(' ')).Count())
            .ToList();
}