using System.Text.RegularExpressions;

namespace day03;

public static class Program
{
    private static void Main()
    {
        var input = ParseInput().ToList();
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static int SolvePartOne(IEnumerable<(int, int, int)> input) => input.Select(x => x.Item3).Sum();

    private static int SolvePartTwo(IEnumerable<(int, int, int)> input)
    {
        var gears = new Dictionary<(int, int), List<int>>();

        foreach (var (x, y, number) in input)
        {
            if (!gears.ContainsKey((x, y)))
            {
                gears.Add((x, y), new List<int>());
            }
            
            gears[(x, y)].Add(number);
        }

        return gears.Where(x => x.Value.Count == 2).Select(x => x.Value.Aggregate(1, (acc, y) => acc * y)).Sum();
    }
    
    private static IEnumerable<(int, int, int)> ParseInput()
    {
        var lines = File.ReadAllLines("input.txt");
        var res = new List<(int, int, int)>();
        var i = 0;

        foreach (var line in lines)
        {
            foreach (Match match in Regex.Matches(line, @"\d+"))
            {
                var number = int.Parse(match.Value);
                var (x0, x1) = (match.Index, match.Index + match.Length);

                for (var y = Math.Max(i - 1, 0); y < Math.Min(i + 2, lines.Length); y++)
                {
                    var sub = lines[y][Math.Max(x0 - 1, 0)..Math.Min(x1 + 1, lines[y].Length)];
                    
                    foreach (Match match1 in Regex.Matches(sub, @"[^\s\d.]"))
                    {
                        res.Add((match1.Index + Math.Max(x0 - 1, 0), y, number));
                    }
                }
            }

            i++;
        }

        return res;
    }
}