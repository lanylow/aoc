namespace day13;

public static class Program
{
    private static void Main()
    {
        var input = File.ReadAllText("input.txt").Split($"{Environment.NewLine}{Environment.NewLine}");
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static long SolvePartOne(IEnumerable<string> input) => input.Select(x => 
        Solve(x, (y, z) => y.Reverse().Zip(z).All(a => a.First == a.Second))
    ).Sum();

    private static long SolvePartTwo(IEnumerable<string> input) => input.Select(x => 
        Solve(x, (y, z) =>
        {
            var a = y.Reverse().Zip(z, (a, b) => a.Zip(b, (c, d) => (c, d)).Where(c => c.c != c.d)).SelectMany(a => a).ToList();
            return a.FirstOrDefault() != default && a.Skip(1).Any() == false;
        })
    ).Sum();

    private static long Solve(string input, Func<string[], string[], bool> compare)
    {
        var rows = input.Split(Environment.NewLine);
        var transposed = Enumerable.Range(0, rows.Select(x => x.Length).Max()).Select(x => string.Concat(rows.Select(y => y[x..(x + 1)]))).ToArray();
        return 100 * FindReflection(rows, compare) + FindReflection(transposed, compare);
    }

    private static long FindReflection(string[] input, Func<string[], string[], bool> compare)
    {
        for (var i = 1; i < input.Length; i++)
        {
            if (compare(input[..i], input[i..]))
            {
                return i;
            }
        }

        return 0;
    }
}