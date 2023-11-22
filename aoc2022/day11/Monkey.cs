namespace day11;

public class Monkey
{
    public Queue<long> Items { get; }
    private Operator Operator { get; }
    public long DivBy { get; }
    private int OnTrue { get; }
    private int OnFalse { get; }
    public long Inspections { get; private set; }

    private Monkey(Queue<long> items, Operator @operator, long divBy, int onTrue, int onFalse, long inspections)
    {
        Items = items;
        Operator = @operator;
        DivBy = divBy;
        OnTrue = onTrue;
        OnFalse = onFalse;
        Inspections = inspections;
    }

    public static Monkey FromString(string str)
    {
        var lines = str.Trim().Split(Environment.NewLine).Skip(1).ToList();
        var items = lines[0].Trim().Replace("Starting items: ", "").Split(", ").Select(long.Parse);
        var exprSplit = lines[1].Trim().Replace("Operation: new = ", "").Split(' ');
        var (opStr, valStr) = ($"{exprSplit[0]} {exprSplit[1]}", exprSplit[2]);
        
        var op = valStr switch
        {
            "old" => opStr switch
            {
                "old +" => new Operator(OperatorType.Add, new Operand(OperandType.Old, 0)),
                "old *" => new Operator(OperatorType.Multiply, new Operand(OperandType.Old, 0)),
                _ => throw new ArgumentOutOfRangeException()
            },
            _ => opStr switch
            {
                "old +" => new Operator(OperatorType.Add, new Operand(OperandType.Value, long.Parse(valStr))),
                "old *" => new Operator(OperatorType.Multiply, new Operand(OperandType.Value, long.Parse(valStr))),
                _ => throw new ArgumentOutOfRangeException()
            }
        };

        var divBy = int.Parse(lines[2].Trim().Replace("Test: divisible by ", ""));
        var onTrue = int.Parse(lines[3].Trim().Replace("If true: throw to monkey ", ""));
        var onFalse = int.Parse(lines[4].Trim().Replace("If false: throw to monkey ", ""));

        return new Monkey(new Queue<long>(items), op, divBy, onTrue, onFalse, 0);
    }

    public List<(int, long)> DoTurn(Func<long, long> worryLevelCallback)
    {
        var throws = new List<(int, long)>();

        while (Items.Count != 0)
        {
            var item = Items.Dequeue();

            var newWorryLevel = Operator.OperatorType switch
            {
                OperatorType.Add => Operator.Operand.OperandType switch
                {
                    OperandType.Old => item + item,
                    OperandType.Value => item + Operator.Operand.Value,
                    _ => throw new ArgumentOutOfRangeException()
                },
                OperatorType.Multiply => Operator.Operand.OperandType switch
                {
                    OperandType.Old => item * item,
                    OperandType.Value => item * Operator.Operand.Value,
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };

            newWorryLevel = worryLevelCallback(newWorryLevel);
            throws.Add(newWorryLevel % DivBy == 0 ? (OnTrue, newWorryLevel) : (OnFalse, newWorryLevel));
            Inspections++;
        }

        return throws;
    }
}