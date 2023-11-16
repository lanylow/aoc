namespace day02;

public abstract class PartOne
{
    public static int Solve()
    {
        var strategy = File.ReadAllLines("input.txt");
        var score = 0;
        
        foreach (var line in strategy)
        {
            var (h1, h2) = ParseLine(line);
            score += h2.PlayAgainst(h1);
        }

        return score;
    }

    private static (Hand, Hand) ParseLine(string str)
    {
        var split = str.Split(" ");
        return (Hand.FromString(split[0]), Hand.FromString(split[1]));
    }
}