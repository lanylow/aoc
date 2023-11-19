namespace day06;

public static class Program
{
    private static void Main()
    {
        var packet = File.ReadAllText("input.txt");
     
        Console.WriteLine($"Part 1: {FindStartOfPacket(packet, 4)}");
        Console.WriteLine($"Part 2: {FindStartOfPacket(packet, 14)}");
    }

    private static int FindStartOfPacket(string str, int unique)
    {
        for (var i = unique; i < str.Length; i++)
        {
            var subString = str.Substring(i - unique, unique);
            var hashSet = new HashSet<char>(subString);
            
            if (hashSet.Count == unique)
            {
                return i;
            }
        }

        return 0;
    }
}