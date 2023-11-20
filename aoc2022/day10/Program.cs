namespace day10;

public static class Program
{
    private static void Main()
    {
        var instructions = File.ReadAllLines("input.txt").Select(Instruction.FromString).ToList();
        var registerValues = GetRegisterValues(instructions);
        
        Console.WriteLine($"Part 1: {SolvePartOne(registerValues)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(registerValues)}");
    }

    private static int SolvePartOne(IReadOnlyList<int> registerValues)
    {
        var sum = 0;
        
        for (var i = 20; i <= 220; i += 40)
        {
            sum += i * registerValues[i - 1];
        }

        return sum;
    }

    private static string SolvePartTwo(IReadOnlyList<int> registerValues)
    {
        const int width = 40;
        const int height = 6;
        var screen = new char[height, width];

        for (var i = 0; i < registerValues.Count; i++)
        {
            var value = registerValues[i];
            var row = i / width;
            var col = i % width;

            screen[row, col] = Math.Abs(col - value) <= 1 ? '#' : '.';
        }

        var result = Environment.NewLine;

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                result += screen[i, j];
            }

            result += Environment.NewLine;
        }

        return result;
    }

    private static List<int> GetRegisterValues(IEnumerable<Instruction> instructions)
    {
        var values = new List<int>();
        var x = 1;

        foreach (var instruction in instructions)
        {
            switch (instruction.Type)
            {
                case InstructionType.Noop:
                {
                    values.Add(x);
                    break;
                }

                case InstructionType.Addx:
                {
                    values.Add(x);
                    values.Add(x);
                    x += instruction.Operand;
                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        return values;
    }
}