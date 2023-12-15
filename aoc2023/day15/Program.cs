using System.Text;

namespace day15;

public static class Program
{
    private static void Main()
    {
        var input = File.ReadAllText("input.txt").Trim();
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static int SolvePartOne(string input) => input.Split(",").Select(Hash).Sum();

    private static int SolvePartTwo(string input)
    {
        var lenses = new Dictionary<string, (int, int)>();
        var instructions = input.Split(",");

        for (var i = 0; i < instructions.Length; i++)
        {
            var x = instructions[i];
            
            if (x[^1] == '-')
            {
                lenses.Remove(x[..^1]);
            }
            else
            {
                var split = x.Split("=");

                var lens = split[0];
                var value = int.Parse(split[1]);

                if (!lenses.TryAdd(lens, (value, i)))
                {
                    lenses[lens] = (value, lenses[lens].Item2);
                }
            }
        }

        var boxes = Enumerable.Repeat(0, 256).Select(_ => new List<(int, int)>()).ToArray();
        
        foreach (var lens in lenses)
        {
            boxes[Hash(lens.Key)].Add(lens.Value);
        }

        return boxes.Select((x, i) => (i + 1) * x.OrderBy(y => y.Item2).Select((y, j) => (j + 1) * y.Item1).Sum()).Sum();
    }
    
    private static int Hash(string str) => str.ToCharArray().Aggregate(0, (x, y) => 17 * (x + y) % 256);
}