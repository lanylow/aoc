namespace day07;

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
        var v = input.Select(x =>
        {
            var split = x.Split(" ");
            return (GetRank(split[0], split[0], "AKQJT98765432"), long.Parse(split[1]));
        }).OrderByDescending(x => x.Item1).ToList();

        return v.Select((x, y) => x.Item2 * (v.Count - y)).Sum();
    }

    private static long SolvePartTwo(IEnumerable<string> input)
    {
        var v = input.Select(x =>
        {
            var split = x.Split(" ");
            return ("23456789TJQKA".Select(y => GetRank(split[0].Replace("J", y.ToString()), split[0], "AKQT98765432J")).Max(), long.Parse(split[1]));
        }).OrderByDescending(x => x.Item1).ToList();

        return v.Select((x, y) => x.Item2 * (v.Count - y)).Sum();
    }

    private static long GetRank(string str, string org, string cards) => GetRank(str) * 16777216 + org.Aggregate(0, (y, z) => y * 16 + (14 - cards.IndexOf(z)));
    
    private static long GetRank(string str)
    {
        var hand = str.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count()).Values.OrderByDescending(x => x).ToList();
     
        if (hand.SequenceEqual(new[] { 5 }))
        {
            return 7;
        }

        if (hand.SequenceEqual(new[] { 4, 1 }))
        {
            return 6;
        }

        if (hand.SequenceEqual(new[] { 3, 2 }))
        {
            return 5;
        }

        if (hand.SequenceEqual(new[] { 3, 1, 1 }))
        {
            return 4;
        }

        if (hand.SequenceEqual(new[] { 2, 2, 1 }))
        {
            return 3;
        }

        if (hand.Count == 4 && hand[0] == 2)
        {
            return 2;
        }

        return 1;
    }
}