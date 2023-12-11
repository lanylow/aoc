namespace day11;

public static class Program
{
    private static void Main()
    {
        var input = File.ReadAllLines("input.txt");

        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static long SolvePartOne(string[] input) => SumLengths(input, 2);
    
    private static long SolvePartTwo(string[] input) => SumLengths(input, 1000000);

    private static long SumLengths(string[] input, int expansion)
    {
        var counts = input.Select(x => x.Count(y => y == '#')).ToList();
        var sums = Enumerable.Range(0, input.Max(x => x.Length)).Select(x => input.Count(y => x < y.Length && y[x] == '#')).ToList();
        return SumLengths2(counts, expansion) + SumLengths2(sums, expansion);
    }

    private static long SumLengths2(IReadOnlyList<int> input, int expansion)
    {
        var sum = 0L;

        for (var i = 0; i < input.Count; i++)
        {
            if (input[i] == 0)
            {
                continue;
            }

            var acc = 0L;

            for (var j = i + 1; j < input.Count; j++)
            {
                acc += input[j] != 0 ? 1 : expansion;
                sum += acc * input[i] * input[j];
            }
        }
        
        return sum;
    }
}