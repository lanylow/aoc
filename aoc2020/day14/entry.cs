namespace day14 {
  internal class entry {
    static long parse_long(string v, int s, int l) {
      var i = 0;
      while (i < l && !char.IsDigit(v[i + s])) i++;

      var negative = i > 0 ? v[i + s - 1] == '-' : false;
      long res = 0;

      while (i < l) {
        byte val = (byte)v[i + s];

        if (val >= '0' && val <= '9')
          res = res * 10 + val - '0';

        i++;
      }

      return negative ? -res : res;
    }

    static long parse_long(string v, string s) {
      var i = v.IndexOf(s);
      return parse_long(v, i + s.Length, v.Length - i - s.Length);
    }

    static long parse_long(string v, string s, string e) {
      var i = v.IndexOf(s);
      var j = v.IndexOf(e, i + s.Length);
      return parse_long(v, i + s.Length, j - i - s.Length);
    }

    static int parse_int(string v, string s, string e) {
      return (int)parse_long(v, s, e);
    }

    static void enumerate(Dictionary<long, long> memory, long mask, int i, long addr, long val) {
      if (i < 0) {
        if (!memory.ContainsKey(addr))
          memory.Add(addr, val);
        else
          memory[addr] = val;

        return;
      }

      long b = 1L << i;

      while (b != 0 && (mask & b) == 0) {
        b >>= 1;
        i--;
      }

      enumerate(memory, mask, i - 1, addr, val);
      enumerate(memory, mask, i - 1, addr | b, val);
    }

    static long solve(string[] lines, bool part2) {
      Dictionary<long, long> memory = new();

      var or = 0L;
      var and = 0L;

      foreach (var line in lines) {
        if (line[1] == 'a') {
          var mask = 0x800000000;

          or = 0L;
          and = 0L;

          for (var i = 7; i < line.Length; i++) {
            switch (line[i]) {
              case '1': or |= mask; break;
              case 'X': and |= mask; break;
            }

            mask >>= 1;
          }
        } else {
          long addr = parse_int(line, "[", "]");
          long val = parse_long(line, " = ");

          if (part2) {
            addr = (addr | or) & ~and;
            enumerate(memory, and, 35, addr, val);
          }
          else {
            if (memory.ContainsKey(addr))
              memory[addr] = (val & and) | or;
            else
              memory.Add(addr, (val & and) | or);
          }
        }
      }

      var sum = 0L;

      foreach (var val in memory.Values)
        sum += val;

      return sum;
    }

    static void Main(string[] args) {
      var lines = File.ReadAllLines("input.txt");

      Console.WriteLine("Sum of all values left in memory after it completes (Part 1): " + solve(lines, false));
      Console.WriteLine("Sum of all values left in memory after it completes (Part 2): " + solve(lines, true));
    }
  }
}