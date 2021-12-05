using System;
using System.IO;
using System.Collections.Generic;

namespace day4 {
  internal class entry {
    const int BOARD_SIZE = 5;

    struct table_entry {
      public table_entry(int _value, bool _marked) { value = _value; marked = _marked; won = false; }

      public int value;
      public bool marked;
      public bool won;
    }

    static int[] get_numbers(string[] all_lines) {
      var numbers = all_lines[0].Split(',');
      var ints = new int[numbers.Length];
      for (var i = 0; i < numbers.Length; i++)
        ints[i] = int.Parse(numbers[i]);
      return ints;
    }

    static List<table_entry[][]> get_tables(string[] all_lines) {
      List<string[]> table_lines = new();
      List<string> table_buffer = new();

      for (var i = 2; i < all_lines.Length; i++) {
        var line = all_lines[i];

        if (!line.Length.Equals(0))
          table_buffer.Add(line);
        else {
          table_lines.Add(table_buffer.ToArray());
          table_buffer.Clear();
        }
      }

      table_lines.Add(table_buffer.ToArray());
      table_buffer.Clear();

      List<table_entry[][]> tables = new();

      foreach (var line in table_lines) {
        table_entry[][] entry = new table_entry[BOARD_SIZE][];

        for (var i = 0; i < BOARD_SIZE; i++) {
          entry[i] = new table_entry[BOARD_SIZE];
          List<int> numbers = new();

          foreach (var number in line[i].Trim().Split(' ')) {
            if (number.Length.Equals(0))
              continue;
            var trimmed = number.Trim();
            numbers.Add(int.Parse(trimmed));
          }

          for (var j = 0; j < BOARD_SIZE; j++)
            entry[i][j] = new table_entry(numbers[j], false);
        }

        tables.Add(entry);
      }

      return tables;
    }

    static bool table_won(table_entry[][] table) {
      for (var i = 0; i < BOARD_SIZE; i++) {
        var j = 0;
        while (j < BOARD_SIZE && table[i][j].marked) ++j;
        if (j >= BOARD_SIZE) return true;

        j = 0;
        while (j < BOARD_SIZE && table[j][i].marked) ++j;
        if (j >= BOARD_SIZE) return true;
      }

      return false;
    }

    static int table_sum(table_entry[][] table) {
      var sum = 0;
      for (var i = 0; i < BOARD_SIZE; i++)
        for (var j = 0; j < BOARD_SIZE; j++)
          if (!table[i][j].marked)
            sum += table[i][j].value;
      return sum;
    }

    static (int, int) solve(int[] numbers, table_entry[][][] tables) {
      var current_number = 0;
      var succeded_results = new List<int>();

      while (current_number < numbers.Length) {
        foreach(var table in tables) {
          if (table[0][0].won)
            continue;

          for (int i = 0; i < BOARD_SIZE; i++)
            for (int j = 0; j < BOARD_SIZE; j++)
              if (table[i][j].value == numbers[current_number])
                table[i][j].marked = true;

          if (table_won(table)) {
            succeded_results.Add(table_sum(table) * numbers[current_number]);
            table[0][0].won = true;
          }
        }

        current_number++;
      }

      return (succeded_results[0], succeded_results[succeded_results.Count - 1]);
    }

    static void Main(string[] args) {
      var all_lines = File.ReadAllLines("input.txt");
      var numbers = get_numbers(all_lines);
      var tables = get_tables(all_lines);

      (int first, int last) = solve(numbers, tables.ToArray());

      Console.WriteLine("Final score of the first table (Part 1): " + first);
      Console.WriteLine("Final score of the last table (Part 2): " + last);

      Console.ReadLine();
    }
  }
}
