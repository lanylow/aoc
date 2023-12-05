namespace day05;

public struct Input
{
    public IEnumerable<long> Seeds { get; }
    public IEnumerable<IEnumerable<Range>> Ranges { get; }

    private Input(IEnumerable<long> seeds, IEnumerable<IEnumerable<Range>> ranges)
    {
        Seeds = seeds;
        Ranges = ranges;
    }
    
    public static Input FromString(string str)
    {
        var split = str.Split($"{Environment.NewLine}{Environment.NewLine}");
        var seeds = split.First().Split(" ").Skip(1).Select(long.Parse).ToList();

        var ranges = split.Skip(1).Select(x => x.Split(Environment.NewLine)).Select(x => x.Skip(1).Select(y =>
        {
            var v = y.Split(" ").Select(long.Parse).ToList();
            return new Range(v[1], v[1] + v[2], v[0] - v[1]);
        }).OrderBy(y => y.Start).ToList()).ToList();

        return new Input(seeds, ranges);        
    }
}