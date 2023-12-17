namespace day17;

public static class Program
{
    private static void Main()
    {
        var input = File.ReadAllLines("input.txt");
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static int SolvePartOne(IEnumerable<string> input) => Solve(input, (x, y) =>
    {
        return y switch
        {
            < 3 => new List<Direction> { PrevDirection(x), NextDirection(x), x },
            _ => new List<Direction> { PrevDirection(x), NextDirection(x) }
        };
    }, _ => true);

    private static int SolvePartTwo(IEnumerable<string> input) => Solve(input, (x, y) =>
    {
        return y switch
        {
            < 4 => new List<Direction> { x },
            < 10 => new List<Direction> { PrevDirection(x), NextDirection(x), x },
            _ => new List<Direction> { PrevDirection(x), NextDirection(x) }
        };
    }, x => x >= 4);
    
    private static int Solve(IEnumerable<string> input, Func<Direction, int, List<Direction>> next, Func<int, bool> check)
    {
        var maze = input.Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList()).ToList();
        var set = new SortedSet<(int, (int, int, Direction, int))>();
        var best = new Dictionary<(int, int, Direction, int), int>();

        set.Add((0, (0, 0, Direction.Right, 0)));
        best.Add((0, 0, Direction.Right, 0), 0);
        
        while (set.Count > 0)
        {
            var element = set.Min;
            var (loss, state) = element;
            var (y, x, direction, distance) = state;
            
            set.Remove(element);
            
            if (y == maze.Count - 1 && x == maze[^1].Count - 1 && check(distance))
            {
                return loss;
            }

            if (loss > best[state])
            {
                continue;
            }

            foreach (var direction1 in next(direction, distance))
            {
                var (y1, x1) = Next(y, x, direction1);

                if (y1 < 0 || y1 >= maze.Count || x1 < 0 || x1 >= maze[y1].Count)
                {
                    continue;
                }

                var loss1 = loss + maze[y1][x1];
                var distance1 = direction == direction1 ? distance + 1 : 1;
                var state1 = (y1, x1, direction1, distance1);

                if (best.ContainsKey(state1) && best[state1] <= loss1)
                {
                    continue;
                }

                if (!best.TryAdd(state1, loss1))
                {
                    best[state1] = loss1;
                }
                
                set.Add((loss1, state1));
            }
        }

        return 0;
    }

    private static (int, int) Next(int y, int x, Direction direction)
    {
        return direction switch
        {
            Direction.Up => (y - 1, x),
            Direction.Left => (y, x - 1),
            Direction.Down => (y + 1, x),
            Direction.Right => (y, x + 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private static Direction NextDirection(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Left,
            Direction.Left => Direction.Down,
            Direction.Down => Direction.Right,
            Direction.Right => Direction.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private static Direction PrevDirection(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Left => Direction.Up,
            Direction.Down => Direction.Left,
            Direction.Right => Direction.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}