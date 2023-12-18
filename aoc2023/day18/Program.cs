namespace day18;

public static class Program
{
    private static void Main()
    {
        var input = File.ReadAllLines("input.txt");

        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static long SolvePartOne(IEnumerable<string> input)
    {
        var parsed = input.Select(x =>
        {
            var split = x.Split(" ");

            var direction = split[0] switch
            {
                "U" => Direction.Up,
                "L" => Direction.Left,
                "D" => Direction.Down,
                "R" => Direction.Right,
                _ => throw new ArgumentOutOfRangeException()
            };

            return (direction, int.Parse(split[1]));
        });

        return Solve(parsed);
    }

    private static long SolvePartTwo(IEnumerable<string> input)
    {
        var parsed = input.Select(x =>
        {
            var code = x.Split(" ").Last().Replace("(#", "").Replace(")", "");

            var direction = code.ToCharArray().Last() switch
            {
                '0' => Direction.Right,
                '1' => Direction.Down,
                '2' => Direction.Left,
                '3' => Direction.Up,
                _ => throw new ArgumentOutOfRangeException()
            };

            return (direction, Convert.ToInt32(code[..^1], 16));
        });

        return Solve(parsed);
    }
    
    private static long Solve(IEnumerable<(Direction, int)> input)
    {
        var (y, a, l) = (0L, 0L, 0L);

        foreach (var (direction, count) in input)
        {
            (y, a) = Next(direction, count, y, a);
            l += count;
        }

        return Math.Abs(a) + l / 2 + 1;
    }

    private static (long, long) Next(Direction direction, int count, long y, long a)
    {
        return direction switch
        {
            Direction.Up => (y - count, a),
            Direction.Left => (y, a - y * count),
            Direction.Down => (y + count, a),
            Direction.Right => (y, a + y * count),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}