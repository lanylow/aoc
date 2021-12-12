using System;
using System.IO;
using System.Collections.Generic;

namespace day12 {
  internal class entry {
    static Dictionary<int, HashSet<int>> connections;
    static int start, end;

    static void parse_input(string[] lines) {
      connections = new();
      Dictionary<string, int> map = new();
      var id = 1;

      foreach (var line in lines) {
        var split = line.Split('-');

        if (!map.TryGetValue(split[0], out int i)) {
          if (split[0] == "start")
            start = id;
          else if (split[0] == "end")
            end = id;

          i = id | (char.IsLower(split[0][0]) ? 0 : 256);
          map[split[0]] = i;
          id++;
        }

        if (!map.TryGetValue(split[1], out int j)) {
          if (split[1] == "start")
            start = id;
          else if (split[1] == "end")
            end = id;

          j = id | (char.IsLower(split[1][0]) ? 0 : 256);
          map[split[1]] = j;
          id++;
        }

        if (j != start && i != end) {
          if (!connections.TryGetValue(i, out var cave_connections)) {
            cave_connections = new();
            connections[i] = cave_connections;
          }

          cave_connections.Add(j);
        }

        if (i != start && j != end) {
          if (!connections.TryGetValue(j, out var cave_connections)) {
            cave_connections = new();
            connections[j] = cave_connections;
          }

          cave_connections.Add(i);
        }
      }
    }

    static int part1() {
      HashSet<int[]> closed = new();
      Queue<List<int>> open = new();
      List<int> path = new();

      path.Add(start);
      open.Enqueue(path);
      var cnt = 0;

      while (open.Count > 0) {
        path = open.Dequeue();
        var last_connection = path[path.Count - 1];

        if (last_connection == end) {
          cnt++;
          continue;
        }

        var cave_connections = connections[last_connection];
        path.Add(-1);
        int i;

        foreach(var next in cave_connections) {
          path[path.Count - 1] = next;

          if (((next & 256) == 256 || (i = path.IndexOf(next)) < 0 || i >= path.Count - 1) && closed.Add(path.ToArray()))
            open.Enqueue(new(path));
        }
      }

      return cnt;
    }

    static int part2() {
      HashSet<int[]> closed = new();
      Queue<(List<int>, bool)> open = new();
      List<int> path = new();

      path.Add(start);
      open.Enqueue((path, false));
      var cnt = 0;

      while (open.Count > 0) {
        bool twice;
        (path, twice) = open.Dequeue();
        var last_connection = path[path.Count - 1];

        if (last_connection == end) {
          cnt++;
          continue;
        }

        var cave_connections = connections[last_connection];
        path.Add(-1);
        int i;

        foreach (var next in cave_connections) {
          path[path.Count - 1] = next;
          bool is_small = (next & 256) == 0;
          bool small_not_seen = (i = path.IndexOf(next)) < 0 || i >= path.Count - 1;

          if ((!twice || !is_small || small_not_seen) && closed.Add(path.ToArray()))
            open.Enqueue((new(path), twice || (is_small && !small_not_seen)));
        }
      }

      return cnt;
    }

    static void Main(string[] args) {
      parse_input(File.ReadAllLines("input.txt"));

      Console.WriteLine("Paths through this cave system that visit small caves at most once (Part 1): " + part1());
      Console.WriteLine("Paths through this cave system (Part 2): " + part2());

      Console.ReadLine();
    }
  }
}
