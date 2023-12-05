namespace day05;

public struct Range
{
    public long Start { get; }
    public long End { get; }
    public long Offset { get; }

    public Range(long start, long end, long offset)
    {
        Start = start;
        End = end;
        Offset = offset;
    }
}