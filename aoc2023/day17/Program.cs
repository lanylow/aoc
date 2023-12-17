namespace day17;

public static class Program
{
    private static void Main()
    {
        var input = File.ReadAllLines("input.txt").Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToArray()).ToArray();
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static int SolvePartOne(IReadOnlyList<int[]> input) => Solve(input, 3, 0);

    private static int SolvePartTwo(IReadOnlyList<int[]> input) => Solve(input, 10, 4);
    
    private static int Solve(IReadOnlyList<int[]> input, int maxDistance, int minDistance)
    {
        var set = new SortedSet<(int, int, int, int, int, int)>();
        var seen = new HashSet<(int, int, int, int, int)>();

        set.Add((0, 0, 0, 0, 0, 0));

        var (height, width) = (input.Count, input[0].Length);

        while (set.Count > 0)
        {
            var (loss, x, y, xd, yd, d) = set.Min;
            set.Remove(set.Min);

            if (!seen.Add((x, y, xd, yd, d)))
            {
                continue;
            }

            if (x == width - 1 && y == height - 1 && d >= minDistance)
            {
                return loss;
            }

            if (d >= minDistance || d == 0)
            {
                if (xd == 0)
                {
                    foreach (var i in new[] { 1, -1 })
                    {
                        if (x + i >= 0 && x + i < width)
                        {
                            set.Add((loss + input[y][x + i], x + i, y, i, 0, 1));
                        }
                    }
                }

                if (yd == 0)
                {
                    foreach (var i in new[] { 1, -1 })
                    {
                        if (y + i >= 0 && y + i < height)
                        {
                            set.Add((loss + input[y + i][x], x, y + i, 0, i, 1));
                        }
                    }
                }
            }

            if (d >= maxDistance)
            {
                continue;
            }
            
            var (x1, y1) = (x + xd, y + yd);

            if (x1 >= 0 && x1 < height && y1 >= 0 && y1 < width)
            {
                set.Add((loss + input[y1][x1], x1, y1, xd, yd, d + 1));
            }
        }

        return 0;
    }
}