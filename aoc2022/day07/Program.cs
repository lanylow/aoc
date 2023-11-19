namespace day07;

public static class Program
{
    private static void Main()
    {
        var input = ParseInput().ToList();
        
        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 1: {SolvePartTwo(input)}");
    }

    private static int SolvePartOne(IEnumerable<int> input)
    {
        return input.Where(x => x <= 100000).Sum();
    }

    private static int SolvePartTwo(IReadOnlyCollection<int> input)
    {
        const int total = 70000000;
        const int required = 30000000;
        
        var used = input.Last();
        var unused = total - used;

        return input.Where(x => x + unused >= required).Take(1).First();
    }
    
    private static IEnumerable<int> ParseInput()
    {
        var input = File.ReadAllLines("input.txt");
        var sizeMap = new Dictionary<string, int>();
        var path = new FilesystemPath();

        foreach (var line in input)
        {
            if (line.StartsWith("$ cd "))
            {
                var directoryName = line.Split(' ')[2];

                if (directoryName == "..")
                {
                    path.Out();
                }
                else
                {
                    path.In(directoryName);
                }
            }
            else if (line.StartsWith("$ ls") || line.StartsWith("dir "))
            {
                
            }
            else
            {
                var size = int.Parse(line.Split(' ')[0]);
                var clone = path.Clone();

                while (true)
                {
                    var pathStr = clone.ToString();
                    
                    if (!sizeMap.ContainsKey(pathStr))
                    {
                        sizeMap[pathStr] = size;
                    }
                    else
                    {
                        sizeMap[pathStr] += size;
                    }

                    if (!clone.Out())
                    {
                        break;
                    }
                }
            }
        }

        var output = sizeMap.Values.ToList();
        output.Sort();
        return output;
    }
}