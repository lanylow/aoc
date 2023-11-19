namespace day05;

public static class Program
{
    private static void Main()
    {
        Console.WriteLine($"Part 1: {SolvePartOne()}");
        Console.WriteLine($"Part 2: {SolvePartTwo()}");
    }

    private static string SolvePartOne()
    {
        var (stacks, instructions) = ParseInput();
        
        foreach (var instruction in instructions)
        {
            for (var i = 0; i < instruction.Count; i++)
            {
                var fromStack = stacks[instruction.From - 1];
                var toStack = stacks[instruction.To - 1];
                
                var c = fromStack.Last();
                fromStack.RemoveAt(fromStack.Count - 1);
                toStack.Add(c);
            }
        }

        return string.Join("", stacks.Select(x => x.Last()));
    }

    private static string SolvePartTwo()
    {
        var (stacks, instructions) = ParseInput();
        
        foreach (var instruction in instructions)
        {
            var fromStack = stacks[instruction.From - 1];
            var toStack = stacks[instruction.To - 1];
            
            var count = fromStack.Count;
            var range = fromStack.GetRange(count - instruction.Count, instruction.Count);
            
            fromStack.RemoveRange(count - instruction.Count, instruction.Count);
            toStack.AddRange(range);
        }
        
        return string.Join("", stacks.Select(x => x.Last()));
    }
    
    private static (List<List<char>>, IEnumerable<Instruction>) ParseInput()
    {
        var input = File.ReadAllText("input.txt");
        var split = input.Split($"{Environment.NewLine}{Environment.NewLine}");

        var stacks = ParseStacks(split[0]);
        var instructions = split[1].Trim().Split(Environment.NewLine).Select(Instruction.FromString);

        return (stacks, instructions);
    }
    
    private static List<List<char>> ParseStacks(string str)
    {
        var stackLines = str.Split(Environment.NewLine).ToList();
        var labelLine = stackLines.Last();
        var stackCount = (labelLine.Length + 2) / 4;
        
        stackLines.RemoveAt(stackLines.Count - 1);
        stackLines.Reverse();

        var stackRows = stackLines.Select(x => ParseRow(x, stackCount)).ToList();
        var output = Enumerable.Range(0, stackCount).Select(_ => new List<char>()).ToList();
        
        foreach (var row in stackRows)
        {
            for (var i = 0; i < row.Count; i++)
            {
                if (row[i].HasValue)
                {
                    output[i].Add(row[i]!.Value);
                }
            }
        }

        return output;
    }

    private static List<char?> ParseRow(string str, int count)
    {
        var width = count * 4 - 1;
        var padded = string.Format($"{{0,-{width}}}", str);
        var output = new List<char?>();

        for (var i = 1; i < width; i += 4)
        {
            var c = padded[i];
            output.Add(c == ' ' ? null : c);
        }

        return output;
    }
}