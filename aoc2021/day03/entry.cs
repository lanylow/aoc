using System;
using System.Collections.Generic;
using System.IO;

namespace day3 {
  internal class entry {
    static (char, char) get_common_bits(string[] lines, int column) {
      var zeros = 0;
      var ones = 0;

      foreach (var line in lines) {
        if (line[column] == '0')
          zeros++;
        else if (line[column] == '1')
          ones++;
      }

      if (zeros > ones)
        return ('0', '1');
      else
        return ('1', '0');
    }

    static (int, int) get_rates(string[] lines) {
      var line_length = lines[0].Length;
      var gamma_bits = new char[line_length];
      var epsilon_bits = new char[line_length];

      for (var i = 0; i < line_length; i++) {
        (char most_common, char least_common) = get_common_bits(lines, i);
        gamma_bits[i] = most_common;
        epsilon_bits[i] = least_common;
      }

      return (Convert.ToInt32(new string(gamma_bits), 2), Convert.ToInt32(new string(epsilon_bits), 2));
    }

    static int get_rating(string[] lines, bool most_bits = false) {
      List<string> numbers = new(lines);

      for (var i = 0; i < lines[0].Length; i++) {
        if (numbers.Count == 1)
          break;

        (char most_common, char least_common) = get_common_bits(numbers.ToArray(), i);

        List<string> buffer = new(numbers);

        foreach (var number in numbers)
          if (number[i] != (most_bits ? most_common : least_common))
            buffer.Remove(number);

        numbers = buffer;
      }

      return Convert.ToInt32(numbers[0], 2);
    }

    static (int, int) get_ratings(string[] lines) {
      return (get_rating(lines, true), get_rating(lines, false));
    }

    static void Main(string[] args) {
      var all_lines = File.ReadAllLines("input.txt");

      (int gamma_rate, int epsilon_rate) = get_rates(all_lines);

      Console.WriteLine("Power consumption of the submarine (Part 1): " + (gamma_rate * epsilon_rate));

      (int oxygen_rating, int scrubber_rating) = get_ratings(all_lines);

      Console.WriteLine("Life support rating of the submarine (Part 2): " + (oxygen_rating * scrubber_rating));

      Console.ReadLine();
    }
  }
}
