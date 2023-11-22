namespace day11;

public static class Program
{
    private static void Main()
    {
        Console.WriteLine($"Part 1: {GetMonkeyBusinessLevel(20)}");
        Console.WriteLine($"Part 2: {GetMonkeyBusinessLevel(10000)}");
    }

    private static long GetMonkeyBusinessLevel(int rounds)
    {
        var monkeys = File.ReadAllText("input.txt").Split($"{Environment.NewLine}{Environment.NewLine}").Select(Monkey.FromString).ToList();
        var factor = monkeys.Select(x => x.DivBy).Aggregate((acc, x) => acc * x);

        for (var i = 0; i < rounds; i++)
        {
            foreach (var moves in monkeys.Select(t => rounds > 20 ? t.DoTurn(x => x % factor) : t.DoTurn(x => x / 3)))
            {
                foreach (var (to, item) in moves)
                {
                    monkeys[to].Items.Enqueue(item);
                }
            }
        }

        var inspections = monkeys.Select(x => x.Inspections).OrderByDescending(x => x).ToList();
        return inspections[0] * inspections[1];
    }
}