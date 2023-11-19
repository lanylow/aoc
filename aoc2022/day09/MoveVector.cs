namespace day09;

public struct MoveVector
{
    public int Count { get; }
    public int Row { get; }
    public int Col { get; }

    public MoveVector(int count, int row, int col)
    {
        Count = count;
        Row = row;
        Col = col;
    }
}