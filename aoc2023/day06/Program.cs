namespace day06;

public static class Program
{
    private static void Main()
    {
        var input = File.ReadAllLines("input.txt");
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static long SolvePartOne(IReadOnlyList<string> input) => input[0]
        .Split(" ")
        .Skip(1)
        .Where(x => x.Trim() != string.Empty)
        .Select(long.Parse)
        .Zip(input[1]
            .Split(" ")
            .Skip(1)
            .Where(x => x.Trim() != string.Empty)
            .Select(long.Parse))
        .Select(x => GetRecordTime(x.First, x.Second))
        .Aggregate(1L, (x, y) => x * y);

    private static long SolvePartTwo(IReadOnlyList<string> input) => GetRecordTime(CombineNumbers(input[0]), CombineNumbers(input[1]));

    private static long CombineNumbers(string str) => long.Parse(str.Split(" ").Skip(1).Where(x => x.Trim() != string.Empty).Aggregate("", (x, y) => x + y));
    
    private static long GetRecordTime(long time, long distance)
    {
        var a = (double)time / 2;
        var b = Math.Sqrt(a * a - distance);
        return (long)(Math.Ceiling(a + b - 1) - Math.Floor(a - b + 1) + 1);
    }
}