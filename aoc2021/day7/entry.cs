using System;
using System.IO;
using System.Linq;

namespace day7 {
  internal class entry {
    static int get_used_fuel_after_moving(int[] crabs, int position) {
      var used = 0;

      foreach (var crab in crabs)
        used += Math.Abs(crab - position);    

      return used;
    }

    static int get_used_fuel_after_moving_step(int[] crabs, int position) {
      var total_used = 0;

      foreach (var crab in crabs) {
        var used = 0;
        var move = 0;
        var sign = Math.Sign(crab - position);
        var tmp = crab;

        while (tmp != position) {
          move++;
          used += move;
          tmp += -sign;
        }

        total_used += used;
      }

      return total_used;
    }

    static int get_cheapest_aligning(int[] crabs, bool step = false) {
      var lowest = int.MaxValue;

      for (var i = 0; i < crabs.Max(); i++) {
        var used_fuel = step ? get_used_fuel_after_moving_step(crabs, i) : get_used_fuel_after_moving(crabs, i);

        if (used_fuel < lowest)
          lowest = used_fuel;
      }

      return lowest;
    }

    static void Main(string[] args) {
      var all_lines = File.ReadAllLines("input.txt");
      var split = all_lines[0].Split(',');
      var crabs = new int[split.Length];

      for (var i = 0; i < split.Length; i++)
        crabs[i] = int.Parse(split[i]);

      get_used_fuel_after_moving_step(crabs, 5);

      Console.WriteLine("Fuel spent aligning to a horizontal position (Part 1): " + get_cheapest_aligning(crabs));
      Console.WriteLine("Fuel spent aligning to a horizontal position with proper crab engineering (Part 2): " + get_cheapest_aligning(crabs, true));

      Console.ReadLine();
    }
  }
}
