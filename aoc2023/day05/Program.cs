namespace day05;

public static class Program
{
    private static void Main()
    {
        var input = Input.FromString(File.ReadAllText("input.txt"));

        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static long SolvePartOne(Input input) => GetLowestLocation(input.Seeds.Select(x => (x, x + 1)), input);

    private static long SolvePartTwo(Input input) => GetLowestLocation(input.Seeds.Chunk(2).Select(x => (x[0], x[0] + x[1])), input);
    
    private static long GetLowestLocation(IEnumerable<(long, long)> init, Input input) => input.Ranges.Aggregate(init, (x, y) => x.SelectMany(z => MapRange(y, z))).Select(x => x.Item1).Min();

    private static IEnumerable<(long, long)> MapRange(IEnumerable<Range> ranges, (long, long) range)
    {
        var res = new List<(long, long)>();

        var acc = ranges.Where(x => x.Start < range.Item2 && range.Item1 < x.End).Aggregate(range.Item1, (x, y) =>
        {
            var start = Math.Max(x, y.Start);
            var end = Math.Min(range.Item2, y.End);

            if (x < start)
            {
                res.Add((x, start));
            }

            res.Add((start + y.Offset, end + y.Offset));

            return end;
        });

        if (acc < range.Item2)
        {
            res.Add((acc, range.Item2));
        }

        return res;
    }
}