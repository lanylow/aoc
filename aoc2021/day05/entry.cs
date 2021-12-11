using System;
using System.IO;
using System.Collections.Generic;

namespace day5 {
  internal class entry {
    struct vector_2d {
      public vector_2d(int _x, int _y) { x = _x; y = _y; }

      public int x, y;
    }

    static vector_2d[][] get_coordinates(string[] lines) {
      var buffer = new vector_2d[lines.Length][];

      for (var i = 0; i < lines.Length; i++) {
        buffer[i] = new vector_2d[2];
        var coordinates = lines[i].Split(" -> ");
        
        for (var j = 0; j < 2; j++) {
          var xy = coordinates[j].Split(',');
          buffer[i][j] = new vector_2d(int.Parse(xy[0]), int.Parse(xy[1]));
        }
      }

      return buffer;
    }

    static Dictionary<(int, int), int> get_points(vector_2d[][] coordinates, bool only_straight = true) {
      Dictionary<(int, int), int> buffer = new();

      foreach (var coord in coordinates) {
        var x1 = coord[0].x;
        var y1 = coord[0].y;
        var x2 = coord[1].x;
        var y2 = coord[1].y;

        if (only_straight && x1 != x2 && y1 != y2)
          continue;

        var xdiff = Math.Sign(x2 - x1);
        var ydiff = Math.Sign(y2 - y1);

        for ((int x, int y) vent = (x1, y1); vent.x != x2 + xdiff || vent.y != y2 + ydiff; vent.x += xdiff, vent.y += ydiff) {
          if (!buffer.ContainsKey(vent))
            buffer[vent] = 0;
          buffer[vent]++;
        }
      }

      return buffer;
    }

    static int count_overlapping_points(Dictionary<(int, int), int> points, int min) {
      var overlapping = 0;
      foreach (var point in points)
        if (point.Value >= min)
          overlapping++;
      return overlapping;
    }

    static void Main(string[] args) {
      var all_lines = File.ReadAllLines("input.txt");
      var coordinates = get_coordinates(all_lines);

      Console.WriteLine("Points where at least two lines overlap (Part 1): " + count_overlapping_points(get_points(coordinates), 2));
      Console.WriteLine("Points where at least two lines overlap diagonally (Part 2): " + count_overlapping_points(get_points(coordinates, false), 2));

      Console.ReadLine();
    }
  }
}
