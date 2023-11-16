namespace day02;

public abstract class PartTwo
{
    public static int Solve()
    {
        var strategy = File.ReadAllLines("input.txt");
        var score = 0;
        
        foreach (var line in strategy)
        {
            var (h, o) = ParseLine(line);
            score += o.PlayAgainst(h);
        }

        return score;
    }

    private static (Hand, Outcome) ParseLine(string str)
    {
        var split = str.Split(" ");
        return (Hand.FromString(split[0]), Outcome.FromString(split[1]));
    }
}