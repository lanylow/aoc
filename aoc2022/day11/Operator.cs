namespace day11;

public struct Operator
{
    public OperatorType OperatorType { get; }
    public Operand Operand { get; }
    
    public Operator(OperatorType operatorType, Operand operand)
    {
        OperatorType = operatorType;
        Operand = operand;
    }
}