namespace day16;

public static class Program
{
    private static readonly Dictionary<(Direction, char), Direction[]> DirectionsMap = new()
    {
        { (Direction.Up, '/'), new[] { Direction.Right } },
        { (Direction.Up, '\\'), new[] { Direction.Left } },
        { (Direction.Up, '-'), new[] { Direction.Left, Direction.Right } },

        { (Direction.Left, '/'), new[] { Direction.Down } },
        { (Direction.Left, '\\'), new[] { Direction.Up } },
        { (Direction.Left, '|'), new[] { Direction.Down, Direction.Up } },

        { (Direction.Down, '/'), new[] { Direction.Left } },
        { (Direction.Down, '\\'), new[] { Direction.Right } },
        { (Direction.Down, '-'), new[] { Direction.Left, Direction.Right } },

        { (Direction.Right, '/'), new[] { Direction.Up } },
        { (Direction.Right, '\\'), new[] { Direction.Down } },
        { (Direction.Right, '|'), new[] { Direction.Down, Direction.Up } }
    };
    
    private static void Main()
    {
        var input = File.ReadAllLines("input.txt");
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static int SolvePartOne(IReadOnlyList<string> input) => Simulate(input, 0, 0, Direction.Right);

    private static int SolvePartTwo(IReadOnlyList<string> input) => 
        Enumerable.Range(0, input.Count)
            .Select(y => (y, 0, Direction.Right))
            .Concat(Enumerable.Range(0, input[0].Length).Select(x => (0, x, Direction.Down)))
            .Concat(Enumerable.Range(0, input.Count).Where(y => y > 0).Select(y => (y, input[y].Length - 1, Direction.Left)))
            .Concat(Enumerable.Range(0, input[^1].Length).Select(x => (input.Count - 1, x, Direction.Up)))
            .AsParallel()
            .Select(x => Simulate(input, x.Item1, x.Item2, x.Item3))
            .Max();
    
    private static int Simulate(IReadOnlyList<string> input, int y, int x, Direction d)
    {
        var stack = new Stack<(int, int, Direction)>();
        var covered = new HashSet<(int, int, Direction)>();
        
        stack.Push((y, x, d));
        covered.Add((y, x, d));

        while (stack.Count > 0)
        {
            var (y1, x1, d1) = stack.Pop();

            foreach (var d2 in DirectionsMap.TryGetValue((d1, input[y1][x1]), out var res) ? res : new[] { d1 })
            {
                var (y2, x2) = Next(y1, x1, d2);

                if (y2 < 0 || y2 >= input.Count || x2 < 0 || x2 >= input[0].Length || covered.Contains((y2, x2, d2)))
                {
                    continue;
                }
                
                stack.Push((y2, x2, d2));
                covered.Add((y2, x2, d2));
            }
        }

        return covered.Select(a => (a.Item1, a.Item2)).Distinct().Count();
    }

    private static (int, int) Next(int y, int x, Direction d)
    {
        return d switch
        {
            Direction.Up => (y - 1, x),
            Direction.Left => (y, x - 1),
            Direction.Down => (y + 1, x),
            Direction.Right => (y, x + 1),
            _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
        };
    }
}