namespace day05;

public struct Instruction
{
    public int Count { get; }
    public int From { get; }
    public int To { get; }
    
    private Instruction(int count, int from, int to)
    {
        Count = count;
        From = from;
        To = to;
    }

    public static Instruction FromString(string str)
    {
        var split = str.Split(' ');
        return new Instruction(int.Parse(split[1]), int.Parse(split[3]), int.Parse(split[5]));
    }
}