using System;
using System.IO;
using System.Collections.Generic;

namespace day9 {
  internal class entry {
    static int width, height;
    static int[] map;
    static List<int> low_points;

    struct basin {
      public int position;
      public int area;
    }

    static void parse_input(string[] lines) {
      width = lines[0].Length;
      height = lines.Length;
      map = new int[lines[0].Length * lines.Length];
      low_points = new();

      var pos = 0;

      foreach (var line in lines)
        foreach (var num in line)
          map[pos++] = num - '0';
    }

    static int get_risk_at(int i) {
      var x = i % width;
      var y = i / width;
      int v = map[i];

      return (x - 1 < 0 || map[i - 1] > v) && (x + 1 >= width || map[i + 1] > v) && (y - 1 < 0 || map[i - width] > v) && (y + 1 >= height || map[i + width] > v) ? v + 1 : 0;
    }

    static int get_risk_of_all_low_points() {
      var res = 0;

      for (var i = 0; i < map.Length; i++) {
        var risk = get_risk_at(i);

        if (risk > 0)
          low_points.Add(i);

        res += risk;
      }

      return res;
    }

    static void get_basin(int pos, ref basin output) {
      if (map[pos] == 9)
        return;

      map[pos] = 9;
      output.area++;

      var x = pos % width;
      var y = pos / width;

      if (x + 1 < width)
        get_basin(pos + 1, ref output);

      if (x - 1 >= 0)
        get_basin(pos - 1, ref output);

      if (y - 1 >= 0)
        get_basin(pos - width, ref output);

      if (y + 1 < height)
        get_basin(pos + width, ref output);
    }

    static basin[] get_all_basins() {
      var res = new basin[low_points.Count];

      for (var i = 0; i < low_points.Count; i++) {
        var cur = new basin() {
          position = low_points[i],
          area = 0
        };

        get_basin(cur.position, ref cur);

        res[i] = cur;
      }

      Array.Sort(res, (basin lhs, basin rhs) => { return rhs.area.CompareTo(lhs.area); });

      return res;
    }

    static void Main(string[] args) {
      parse_input(File.ReadAllLines("input.txt"));

      Console.WriteLine("Sum of the risk levels of all low points (Part 1): " + get_risk_of_all_low_points());

      var basins = get_all_basins();

      Console.WriteLine("Multiplied size of the three largest basins (Part 2): " + (basins[0].area * basins[1].area * basins[2].area));

      Console.ReadLine();
    }
  }
}
