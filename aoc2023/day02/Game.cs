using System.Text.RegularExpressions;

namespace day02;

public class Game
{
    public int Id { get; }
    public List<(int, Color)> Cubes { get; }

    private Game(int id, List<(int, Color)> cubes)
    {
        Id = id;
        Cubes = cubes;
    }

    public static Game FromString(string str)
    {
        var id = int.Parse(str.Split(' ')[1][..^1]);
        var cubes = new List<(int, Color)>();

        foreach (Match match in Regex.Matches(str, "(\\d+) ((?:red)|(?:green)|(?:blue))"))
        {
            var color = match.Groups[2].Value switch
            {
                "red" => Color.Red,
                "green" => Color.Green,
                "blue" => Color.Blue,
                _ => throw new ArgumentOutOfRangeException()
            };

            var count = int.Parse(match.Groups[1].Value);
            
            cubes.Add((count, color));
        }
        
        return new Game(id, cubes);
    }

    public bool IsPossible() => Cubes.All(x => x.Item1 <= GetMaxColorCount(x.Item2));

    private static int GetMaxColorCount(Color color)
    {
        return color switch
        {
            Color.Red => 12,
            Color.Green => 13,
            Color.Blue => 14,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }
}