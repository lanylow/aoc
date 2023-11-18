namespace day04;

public static class Program
{
    private static void Main()
    {
        var (partOne, partTwo) = Solve();
        
        Console.WriteLine($"Part 1: {partOne}");
        Console.WriteLine($"Part 2: {partTwo}");
    }

    private static (int, int) Solve()
    {
        var assignments = File.ReadAllLines("input.txt");
        
        var fullyContained = 0;
        var overlapping = 0;
        
        foreach (var assignment in assignments)
        {
            var (r1, r2) = ParseLine(assignment);

            if (r1.FullyContains(r2))
            {
                fullyContained++;
            }

            if (r1.Overlaps(r2))
            {
                overlapping++;
            }
        }

        return (fullyContained, overlapping);
    }

    private static (Range, Range) ParseLine(string str)
    {
        var split = str.Trim().Split(',');
        return (Range.FromString(split[0]), Range.FromString(split[1]));
    }
}