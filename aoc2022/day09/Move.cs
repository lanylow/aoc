namespace day09;

public class Move
{
    private MoveType MoveType { get; }
    private int Count { get; }
    
    private Move(MoveType moveType, int count)
    {
        MoveType = moveType;
        Count = count;
    }

    public MoveVector GetMoveVector()
    {
        return MoveType switch
        {
            MoveType.Left => new MoveVector(Count, 0, -1),
            MoveType.Right => new MoveVector(Count, 0, 1),
            MoveType.Up => new MoveVector(Count, 1, 0),
            MoveType.Down => new MoveVector(Count, -1, 0),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static Move FromString(string str)
    {
        var split = str.Split(' ');
        var count = int.Parse(split[1]);

        var type = split[0] switch
        {
            "L" => MoveType.Left,
            "R" => MoveType.Right,
            "U" => MoveType.Up,
            "D" => MoveType.Down,
            _ => throw new ArgumentOutOfRangeException()
        };

        return new Move(type, count);
    }
}