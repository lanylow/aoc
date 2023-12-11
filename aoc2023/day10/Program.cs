using System.Collections;

namespace day10;

public static class Program
{
    private static readonly Dictionary<(Direction, char), Direction> Directions = new()
    {
        { (Direction.Up, '|'), Direction.Up },
        { (Direction.Up, '7'), Direction.Left },
        { (Direction.Up, 'F'), Direction.Right },
        { (Direction.Left, '-'), Direction.Left },
        { (Direction.Left, 'F'), Direction.Down },
        { (Direction.Left, 'L'), Direction.Up },
        { (Direction.Down, '|'), Direction.Down },
        { (Direction.Down, 'L'), Direction.Right },
        { (Direction.Down, 'J'), Direction.Left },
        { (Direction.Right, '-'), Direction.Right },
        { (Direction.Right, 'J'), Direction.Up },
        { (Direction.Right, '7'), Direction.Down },
    };
    
    private static void Main()
    {
        var input = ParseInput();
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static int SolvePartOne(ICollection input) => input.Count / 2;

    private static int SolvePartTwo(IReadOnlyCollection<(int, int)> input)
    {
        var res = input.Zip(input.Skip(1).Concat(input), (y, x) => (y, x)).Select(x => (x.x.Item2 * x.y.Item1, x.y.Item2 * x.x.Item1)).Aggregate((0, 0), (x, y) => (x.Item1 + y.Item1, x.Item2 + y.Item2));
        return (2 + Math.Max(res.Item1, res.Item2) - Math.Min(res.Item1, res.Item2) - input.Count) / 2;
    }
    
    private static List<(int, int)> ParseInput()
    {
        var input = File.ReadAllLines("input.txt");
        
        for (var y = 0; y < input.Length; y++)
        {
            var line = input[y];

            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];

                if (c != 'S')
                {
                    continue;
                }

                var startPosition = (y, x);

                foreach (var i in new[] { Direction.Up, Direction.Left, Direction.Down, Direction.Right })
                {
                    var direction = i;
                    var position = startPosition;
                    var path = new List<(int, int)>();

                    try
                    {
                        while (direction != Direction.None)
                        {
                            path.Add(position);
                            position = Next(position, direction);

                            if (position.y < 0 || position.y >= input.Length || position.x < 0 || position.x >= line.Length)
                                throw new IndexOutOfRangeException();

                            direction = Directions.GetValueOrDefault((direction, input[position.y][position.x]), Direction.None);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    if (position == startPosition)
                    {
                        return path;
                    }
                }
            }
        }

        return new List<(int, int)>();
    }

    private static (int, int) Next((int y, int x) position, Direction direction)
    {
        return direction switch
        {
            Direction.Up => (position.y - 1, position.x),
            Direction.Left => (position.y, position.x - 1),
            Direction.Down => (position.y + 1, position.x),
            Direction.Right => (position.y, position.x + 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}