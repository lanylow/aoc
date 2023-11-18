namespace day04;

public class Range
{
    private int Start { get; }
    private int End { get; }

    private Range(int start, int end)
    {
        Start = start;
        End = end;
    }

    public static Range FromString(string str)
    {
        var split = str.Trim().Split('-');
        return new Range(int.Parse(split[0]), int.Parse(split[1]));
    }

    public bool FullyContains(Range other)
    {
        return (other.Start >= Start && other.End <= End) || (Start >= other.Start && End <= other.End);
    }

    public bool Overlaps(Range other)
    {
        var r1 = Start < other.Start ? this : other;
        var r2 = Start > other.Start ? this : other;
        return r1.End >= r2.Start;
    }
}