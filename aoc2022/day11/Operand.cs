namespace day11;

public struct Operand
{
    public OperandType OperandType { get; }
    public long Value { get; }
    
    public Operand(OperandType operandType, long value)
    {
        OperandType = operandType;
        Value = value;
    }
}