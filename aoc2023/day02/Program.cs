namespace day02;

public static class Program
{
    private static void Main()
    {
        var games = File.ReadAllLines("input.txt").Select(Game.FromString).ToList();
        
        Console.WriteLine($"Part 1: {SolvePartOne(games)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(games)}");
    }

    private static int SolvePartOne(IEnumerable<Game> games)
    {
        return games.Where(x => x.IsPossible()).Select(x => x.Id).Sum();
    }

    private static int SolvePartTwo(IEnumerable<Game> games)
    {
        var sum = 0;
        
        foreach (var game in games)
        {
            var power = 1;

            for (var i = 0; i <= 2; i++)
            {
                power *= game.Cubes.Where(x => x.Item2 == (Color)i).Select(x => x.Item1).Max();
            }

            sum += power;
        }

        return sum;
    }
}