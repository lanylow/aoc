namespace day03;

public static class Program
{
    private static void Main()
    {
        Console.WriteLine($"Part 1: {SolvePartOne()}");
        Console.WriteLine($"Part 2: {SolvePartTwo()}");
    }

    private static int SolvePartOne()
    {
        var rucksacks = File.ReadAllLines("input.txt");
        return rucksacks.Sum(ParseRucksack);
    }

    private static int SolvePartTwo()
    {
        var rucksacks = File.ReadAllLines("input.txt");
        var chunks = rucksacks.Chunk(3);

        return (from chunk in chunks 
            let r0 = chunk[0].ToArray() 
            let r1 = chunk[1].ToArray() 
            let r2 = chunk[2].ToArray() 
            select r0.Intersect(r1).Intersect(r2).ToArray() into r0 
            select GetPriority(r0.First())).Sum();
    }

    private static int ParseRucksack(string str)
    {
        var split = str.Length / 2;
        var first = str[..split].ToArray();
        var last = str[split..].ToArray();
        var item = first.Intersect(last).First();
        return GetPriority(item);
    }

    private static int GetPriority(char c)
    {
        var v = c - 96;
        return v > 0 ? v : v + 58;
    }
}