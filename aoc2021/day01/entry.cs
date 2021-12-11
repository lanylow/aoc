using System;
using System.IO;

namespace day1 {
  internal class entry {
    static int get_incresed_measurements(int[] measurements, int window) {
      var incresed = 0;
      for (int i = window; i < measurements.Length; i++) {
        var last_sum = 0;
        var previous_sum = 0;

        for (int j = 0; j < window; j++) {
          last_sum += measurements[i - j];
          previous_sum += measurements[i - j - 1];
        }

        if (last_sum > previous_sum)
          incresed++;
      }
      return incresed;
    }

    static void Main(string[] args) {
      var all_lines = File.ReadAllLines("input.txt");
      var measurements = new int[all_lines.Length];

      for (var i = 0; i < all_lines.Length; i++)
        measurements[i] = int.Parse(all_lines[i]);

      Console.WriteLine("Increased measurements (Part 1): " + get_incresed_measurements(measurements, 1));
      Console.WriteLine("Sums larger than the previous sum (Part 2): " + get_incresed_measurements(measurements, 3));

      Console.ReadLine();
    }
  }
}
