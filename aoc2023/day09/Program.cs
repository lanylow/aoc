namespace day09;

public static class Program
{
    private static void Main()
    {
        var input = File.ReadAllLines("input.txt");
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static long SolvePartOne(IEnumerable<string> input) => input.Select(x => Predict(x.Split(" ").Select(int.Parse).ToArray())).Sum();

    private static long SolvePartTwo(IEnumerable<string> input) => input.Select(x => Predict(x.Split(" ").Select(int.Parse).Reverse().ToArray())).Sum();
    
    private static long Predict(IReadOnlyCollection<int> values) => values.Select((x, i) => (i, x)).Aggregate((1L, 0L), (x, y) => (x.Item1 * (values.Count - y.i) / (y.i + 1), x.Item1 * y.x - x.Item2)).Item2;
}