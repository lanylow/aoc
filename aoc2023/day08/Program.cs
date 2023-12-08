namespace day08;

public static class Program
{
    private static void Main()
    {
        var input = ParseInput();
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static long SolvePartOne(Network network) => network.Instructions.Count * Unfold("AAA", network.Next).TakeWhile(x => x != "ZZZ").Count();

    private static long SolvePartTwo(Network network) => network.Instructions.Count * network.Table.Keys
        .Where(x => x.EndsWith("A"))
        .Select(x => 
        {
            var (index, end) = Unfold(x, network.Next).Select((end, index) => (index, end)).FirstOrDefault(y => y.end.EndsWith("Z"));

            if (network.Next(x) == network.Next(end))
            {
                return index;
            }
            
            return -1L;
        })
        .Where(x => x != -1)
        .Aggregate(1L, GetLowestCommonMultiple);
    
    private static Network ParseInput()
    {
        var input = File.ReadAllLines("input.txt");

        var instructions = input[0].Select(x =>
        {
            return x switch
            {
                'L' => true,
                'R' => false,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
            };
        }).ToList();

        var table = input.Skip(2).Select(x =>
        {
            var y = x.Split(" = ", 2);
            var z = y[1].Split(", ", 2);
            return (y[0], (z[0].Replace("(", ""), z[1].Replace(")", "")));
        }).ToDictionary(x => x.Item1, x => x.Item2);

        return new Network(instructions, table);
    }

    private static long GetGreatestCommonDivisor(long x, long y)
    {
        while (!y.Equals(default))
        {
            var t = y;
            y = x % y;
            x = t;
        }

        return x;
    }

    private static long GetLowestCommonMultiple(long x, long y)
    {
        return x / GetGreatestCommonDivisor(x, y) * y;
    }

    private static IEnumerable<T> Unfold<T>(T seed, Func<T, T> generator)
    {
        var current = seed;

        while (true)
        {
            yield return current;
            current = generator(current);
        }
    }
}