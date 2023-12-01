using System.Text.RegularExpressions;

namespace day01;

public static class Program
{
    private static readonly Dictionary<string, string> NumberMap = new()
    {
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" }
    };
    
    private static void Main()
    {
        Console.WriteLine($"Part 1: {SolvePartOne()}");
        Console.WriteLine($"Part 2: {SolvePartTwo()}");
    }

    private static int SolvePartOne() => SumCalibrationValues(File.ReadAllLines("input.txt"));

    private static int SolvePartTwo() => SumCalibrationValues(File.ReadAllLines("input.txt").Select(ReplaceNumbers));

    private static int SumCalibrationValues(IEnumerable<string> str) => str.Select(x => Regex.Replace(x, "[^0-9]", "")).Select(x => int.Parse(x[0].ToString() + x[^1])).Sum();
    
    private static string ReplaceNumbers(string str) => NumberMap.Aggregate(str, (x, n) => x.Replace(n.Key, $"{n.Key}{n.Value}{n.Key}"));
}