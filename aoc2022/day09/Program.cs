namespace day09;

public static class Program
{
    private static void Main()
    {
        Console.WriteLine($"Part 1: {SimulateTail(2)}");
        Console.WriteLine($"Part 2: {SimulateTail(10)}");
    }

    private static int SimulateTail(int knotCount)
    {
        var input = File.ReadAllLines("input.txt");
        var knots = new(int, int)[knotCount];
        var tailPositions = new HashSet<(int, int)> { knots.Last() };

        foreach (var line in input)
        {
            var move = Move.FromString(line);
            var moveVector = move.GetMoveVector();

            for (var i = 0; i < moveVector.Count; i++)
            {
                knots[0] = (knots[0].Item1 + moveVector.Row, knots[0].Item2 + moveVector.Col);

                for (var j = 1; j < knotCount; j++)
                {
                    var (rowOffset, colOffset) = (knots[j - 1].Item1 - knots[j].Item1, knots[j - 1].Item2 - knots[j].Item2);
                    var (rowOp, colOp) = (Math.Sign(rowOffset), Math.Sign(colOffset));

                    if (Math.Abs(rowOffset) > 1 || Math.Abs(colOffset) > 1)
                    {
                        knots[j] = (knots[j].Item1 + rowOp, knots[j].Item2 + colOp);
                    }
                }

                tailPositions.Add(knots.Last());
            }
        }

        return tailPositions.Count;
    }
}