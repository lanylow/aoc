using System;
using System.IO;
using System.Collections.Generic;

namespace day8 {
  internal class entry {
    static int filter_segments_in_output(string[] input, int[] number_of_segments) {
      var result = 0;

      foreach (var segment in input) {
        foreach (var digit in segment.Split('|')[1].Trim().Split(' '))
          foreach (var number in number_of_segments)
            if (digit.Length == number)
              result++;
      }

      return result;
    }

    static string sort_string(string code) {
      char[] chars = code.ToCharArray();
      Array.Sort(chars);
      return new string(chars);
    }

    static int get_occurences(string lhs, string rhs) {
      int l = 0, r = 0, cnt = 0;

      while (l < lhs.Length && r < rhs.Length) {
        if (lhs[l] == rhs[r]) {
          cnt++;
          l++;
          r++;
        }
        else if (lhs[l] < rhs[r]) {
          l++;
        }
        else {
          r++;
        }
      }

      return cnt;
    }

    static int get_pattern_and_add_up(string[] input) {
      var sum = 0;
      var map = new Dictionary<string, int>();
      var rem = new List<string>();

      foreach (var entry in input) {
        map.Clear();
        rem.Clear();

        var split = entry.Split(' ');
        string one = null, four = null;

        for (var i = 0; i < 10; i++) {
          string sorted = sort_string(split[i]);

          switch (sorted.Length) {
            case 2:
              map[sorted] = 1;
              one = sorted;
              break;

            case 3:
              map[sorted] = 7;
              break;

            case 4:
              map[sorted] = 4;
              four = sorted;
              break;

            case 7:
              map[sorted] = 8;
              break;

            case 5:
            case 6:
              rem.Add(sorted);
              break;
          }
        }

        for (var i = rem.Count - 1; i >= 0; i--) {
          var code = rem[i];

          var ones = get_occurences(one, code);
          var fours = get_occurences(four, code);

          switch (code.Length) {
            case 5:
              if (fours == 2)
                map[code] = 2;
              else if (ones == 2)
                map[code] = 3;
              else
                map[code] = 5;

              break;

            case 6:
              if (fours == 4)
                map[code] = 9;
              else if (ones == 1)
                map[code] = 6;
              else
                map[code] = 0;

              break;
          }
        }

        int val = 0;

        for (var i = 11; i < 15; i++) {
          var code = sort_string(split[i]);
          val = val * 10 + map[code];
        }

        sum += val;
      }

      return sum;
    }

    static void Main(string[] args) {
      var all_lines = File.ReadAllLines("input.txt");

      Console.WriteLine("In the output values, digits 1, 4, 7 or 8 appear " + filter_segments_in_output(all_lines, new int[] { 2, 4, 3, 7 }) + " times (Part 1)");
      Console.WriteLine("Sum of all output numbers (Part 2): " + get_pattern_and_add_up(all_lines));

      Console.ReadLine();
    }
  }
}
