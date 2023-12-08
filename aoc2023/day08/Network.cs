namespace day08;

public class Network
{
    public List<bool> Instructions { get; }
    public Dictionary<string, (string, string)> Table { get; }

    public Network(List<bool> instructions, Dictionary<string, (string, string)> table)
    {
        Instructions = instructions;
        Table = table;
    }

    public string Next(string start) => Instructions.Aggregate(start, (x, y) => y ? Table[x].Item1 : Table[x].Item2);
}