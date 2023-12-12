using System.Collections.Immutable;

namespace day12;

public static class Program
{
    private static readonly Dictionary<(string, ImmutableStack<int>), long> ResultCache = new();
    
    private static void Main()
    {
        var input = File.ReadAllLines("input.txt");

        Console.WriteLine($"Part 1: {SolvePartOne(input)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(input)}");
    }

    private static long SolvePartOne(IEnumerable<string> input) => SolveRepeated(input, 1);

    private static long SolvePartTwo(IEnumerable<string> input) => SolveRepeated(input, 5);

    private static long SolveRepeated(IEnumerable<string> input, int repeat) => (
        from line in input 
        select line.Split(" ") into split 
        let str = string.Join("?", Enumerable.Repeat(split[0], repeat))
        let groups = Enumerable.Repeat(split[1].Split(",").Select(int.Parse), repeat).SelectMany(x => x)
        select SolveCached(str, ImmutableStack.CreateRange(groups.Reverse()), true)).Sum();
    
    private static long SolveCached(string str, ImmutableStack<int> groups, bool resetCache = false)
    {
        if (resetCache)
        {
            ResultCache.Clear();
        } 
        
        if (!ResultCache.ContainsKey((str, groups)))
        {
            ResultCache[(str, groups)] = Solve(str, groups);
        }

        return ResultCache[(str, groups)];
    }
    
    private static long Solve(string str, ImmutableStack<int> groups)
    {
        var count = groups.Select(x => (long)x).Sum();

        if (count < str.Count(x => x == '#') || count > str.Count(x => x != '.') || count + groups.Count() > str.Length + 1)
        {
            return 0;
        }

        if (string.IsNullOrEmpty(str) || !groups.Any())
        {
            return 1;
        }

        var top = groups.Peek();
        count = 0;
        
        if (!str[..top].Contains('.') && !str[top..].StartsWith('#'))
        {
            count += SolveCached(str[Math.Min(top + 1, str.Length)..].Trim('.'), groups.Pop());
        }
        
        if (!str.StartsWith('#'))
        {
            count += SolveCached(str[1..], groups);
        }

        return count;
    }
}