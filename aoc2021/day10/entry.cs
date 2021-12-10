using System;
using System.IO;
using System.Collections.Generic;

namespace day10 {
  internal class entry {
    static bool is_token_opening(char c) => c == '(' || c == '[' || c == '{' || c == '<';

    static bool is_correct_closing_token(char o, char c) => o == '(' && c == ')' || o == '[' && c == ']' || o == '{' && c == '}' || o == '<' && c == '>';

    static char get_correct_closing_type(char o) {
      switch (o) {
        default:
        case '(': return ')';
        case '[': return ']';
        case '{': return '}';
        case '<': return '>';
      }
    }

    static (string[] good_lines, int syntax_error_score) get_total_syntax_error_score(string[] lines) {
      List<char> errors = new();
      List<char> tokens = new();
      List<string> good_lines = new();

      foreach (var line in lines) {
        tokens.Clear();

        for (var i = 0; i < line.Length; i++) {
          var c = line[i];

          if (i == line.Length - 1) {
            good_lines.Add(line);
            break;
          }

          if (is_token_opening(c)) {
            tokens.Add(c);
          }
          else {
            if (is_correct_closing_token((char)tokens[tokens.Count - 1], c)) {
              tokens.RemoveAt(tokens.Count - 1);
            }
            else {
              errors.Add(c);
              break;
            }
          }
        }
      }

      var res = 0;

      foreach (var error in errors) {
        switch (error) {
          case ')': res += 3; break;
          case ']': res += 57; break;
          case '}': res += 1197; break;
          case '>': res += 25137; break;
        }
      }

      return (good_lines.ToArray(), res);
    }

    static long get_middle_score(string[] lines) {
      List<char> tokens = new();
      List<long> results = new();

      foreach (var line in lines) {
        tokens.Clear();

        var buffer = new string(line);
        var res = 0L;

        for (var i = 0;; i++) {
          if (i >= line.Length) {
            var closing_type = get_correct_closing_type(tokens[tokens.Count - 1]);
            buffer += (char)closing_type;
            tokens.RemoveAt(tokens.Count - 1);

            res *= 5;

            switch (closing_type) {
              case ')': res += 1; break;
              case ']': res += 2; break;
              case '}': res += 3; break;
              case '>': res += 4; break;
            }

            if (tokens.Count == 0) {
              results.Add(res);
              break;
            }

            continue;
          }

          var c = line[i];

          if (is_token_opening(c))
            tokens.Add(c);
          else
            tokens.RemoveAt(tokens.Count - 1);
        }
      }

      results.Sort();

      return results[results.Count / 2];
    }

    static void Main(string[] args) {
      var all_lines = File.ReadAllLines("input.txt");

      (string[] good_lines, int syntax_error_score) = get_total_syntax_error_score(all_lines);

      Console.WriteLine("Total syntax error score (Part 1): " + syntax_error_score);
      Console.WriteLine("Middle score of good lines (Part 2): " + get_middle_score(good_lines));

      Console.ReadLine();
    }
  }
}
