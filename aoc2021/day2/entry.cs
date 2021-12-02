using System;
using System.IO;

namespace day2 {
  internal class entry {
    static (int, int, int) get_position(string[] instructions) {
      var depth = 0;
      var horizontal = 0;
      var aim = 0;
      var depth_aim = 0;

      foreach (var instruction in instructions) {
        var split = instruction.Split(' ');
        var direction = split[0];
        var value = int.Parse(split[1]);

        switch (direction) {
          case "up":
            depth -= value;
            aim -= value;
            break;

          case "down":
            depth += value;
            aim += value;
            break;

          case "forward":
            horizontal += value;
            depth_aim += aim * value;
            break;
        }
      }

      return (depth, horizontal, depth_aim);
    }

    static void Main(string[] args) {
      var all_lines = File.ReadAllLines("input.txt");

      (int depth, int horizontal, int depth_aim) = get_position(all_lines);

      Console.WriteLine("Depth multiplied by horizontal position (Part 1): " + (depth * horizontal));
      Console.WriteLine("Aimed depth multiplied by horizontal position (Part 2): " + (depth_aim * horizontal));

      Console.ReadLine();
    }
  }
}
