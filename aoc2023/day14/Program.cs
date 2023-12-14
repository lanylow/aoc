namespace day14;

public static class Program
{
    private static void Main()
    {
        Console.WriteLine($"Part 1: {SolvePartOne()}");
        Console.WriteLine($"Part 2: {SolvePartTwo()}");
    }

    private static long SolvePartOne()
    {
        var (data, width, height) = Parse();
        Tilt(ref data, width, height);
        return CalculateLoad(data);
    }

    private static long SolvePartTwo()
    {
        var (data, width, height) = Parse();
        var cache = new List<string>();

        for (var i = 1000000000; i > 0;) {
            Spin(ref data, width, height);
            i--;
            var hash = string.Join("", from x in data from y in x select y);

            if (!cache.Contains(hash))
            {
                cache.Add(hash);
            }
            else
            {
                i %= cache.Count - cache.IndexOf(hash);

                for (; i > 0; i--)
                {
                    Spin(ref data, width, height);
                }
                
                break;
            }
        }
        
        return CalculateLoad(data);
    }

    private static (List<List<char>>, int, int) Parse()
    {
        var input = File.ReadAllLines("input.txt");
        var lines = input.Select(x => x.ToList()).ToList();
        var width = lines.Select(x => x.Count).Max();
        return (lines, width, lines.Count);
    }
    
    private static void Tilt(ref List<List<char>> data, int width, int height)
    {
        for (var x = 0; x < width; x++)
        {
            var y0 = 0;

            while (y0 < height)
            {
                var n = 0;
                var y1 = y0;

                while (y1 < height)
                {
                    var c = data[y1][x];

                    if (c == 'O')
                    {
                        n += 1;
                    }
                    else if (c == '#')
                    {
                        break;
                    }

                    y1++;
                }

                for (var y = y0; y < y1; y++)
                {
                    data[y][x] = y < y0 + n ? 'O' : '.';
                }

                y0 = y1 + 1;
            }
        }
    }

    private static void Spin(ref List<List<char>> data, int width, int height)
    {
        for (var i = 0; i < 4; i++)
        {
            Tilt(ref data, width, height);
            
            var rotated = new List<List<char>>();
            
            for (var row = 0; row < width; row++)
            {
                rotated.Add(new List<char>());

                for (var col = 0; col < height; col++)
                {
                    rotated[row].Add(data[height - col - 1][row]);
                }
            }

            data = rotated;
        }
    }
    
    private static long CalculateLoad(IEnumerable<List<char>> data) => data.Select((x, i) => (x.Count - i) * x.Count(y => y == 'O')).Sum();
}