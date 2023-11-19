namespace day08;

public static class Program
{
    private static void Main()
    {
        var input = File.ReadAllLines("input.txt");
        var trees = input.Select(line => line.ToList()).ToList();
        
        Console.WriteLine($"Part 1: {SolvePartOne(trees)}");
        Console.WriteLine($"Part 2: {SolvePartTwo(trees)}");
    }

    private static int SolvePartOne(IReadOnlyList<IReadOnlyList<char>> trees)
    {
        var visible = 0;

        for (var i = 0; i < trees.Count; i++)
        {
            for (var j = 0; j < trees[0].Count; j++)
            {
                var height = trees[i][j];
                
                if (trees.Take(i).All(x => x[j] < height) ||
                    trees[i].Take(j).All(x => x < height) ||
                    trees.Skip(i + 1).All(x => x[j] < height) ||
                    trees[i].Skip(j + 1).All(x => x < height))
                {
                    visible++;
                }
            }
        }

        return visible;
    }

    private static int SolvePartTwo(IReadOnlyList<IReadOnlyList<char>> trees)
    {
        var highestScore = 0;
        var rowCount = trees.Count;
        var colCount = trees[0].Count;

        for (var i = 0; i < rowCount; i++)
        {
            for (var j = 0; j < colCount; j++)
            {
                var top = 0;
                var left = 0;
                var bottom = 0;
                var right = 0;
                
                for (var k = i - 1; k >= 0; k--)
                {
                    top++;

                    if (trees[i][j] <= trees[k][j])
                    {
                        break;
                    }
                }
                
                for (var k = j - 1; k >= 0; k--)
                {
                    left++;

                    if (trees[i][j] <= trees[i][k])
                    {
                        break;
                    }
                }
                
                for (var k = i + 1; k < rowCount; k++)
                {
                    bottom++;

                    if (trees[i][j] <= trees[k][j])
                    {
                        break;
                    }
                }
                
                for (var k = j + 1; k < colCount; k++)
                {
                    right++;

                    if (trees[i][j] <= trees[i][k])
                    {
                        break;
                    }
                }
                
                highestScore = Math.Max(highestScore, top * left * bottom * right);
            }
        }

        return highestScore;
    }
}