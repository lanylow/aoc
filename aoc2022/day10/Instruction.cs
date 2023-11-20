namespace day10;

public struct Instruction
{
    public InstructionType Type { get; }
    public int Operand { get; }
    
    private Instruction(InstructionType instructionType, int operand)
    {
        Type = instructionType;
        Operand = operand;
    }

    public static Instruction FromString(string str)
    {
        var split = str.Split(' ');

        var type = split[0] switch
        {
            "noop" => InstructionType.Noop,
            "addx" => InstructionType.Addx,
            _ => throw new ArgumentOutOfRangeException()
        };

        var operand = type == InstructionType.Addx ? int.Parse(split[1]) : 0;

        return new Instruction(type, operand);
    }
}