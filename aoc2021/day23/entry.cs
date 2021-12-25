using System;
using System.IO;
using System.Collections.Generic;

namespace day23 {
  internal class amphipod {
    public static amphipod empty = new();
    public int type;

    public amphipod() { type = -1; }

    public amphipod(int _type) { type = _type; }

    public static bool operator ==(amphipod lhs, amphipod rhs) { return lhs.type == rhs.type; }

    public static bool operator !=(amphipod lhs, amphipod rhs) { return lhs.type != rhs.type; }

    public override bool Equals(object obj) { return obj is amphipod pad && pad == this; }

    public override int GetHashCode() { return type; }
  }

  internal class entry {
    static int[] energies = new[] { 1, 10, 100, 1000 };
    static amphipod[] rooms, hallways;
    static int lowest_energy;

    static void parse(string[] lines) {
      hallways = new amphipod[7];
      rooms = new amphipod[16];

      Array.Fill(hallways, amphipod.empty);
      Array.Fill(rooms, amphipod.empty);

      var l = lines[3];

      for (var i = 3; i < 10; i += 2) {
        var type = l[i] - 'A';
        var room = (i - 3) / 2;
        rooms[room + 4] = new amphipod(type);
      }

      l = lines[2];

      for (var i = 3; i < 10; i += 2) {
        var type = l[i] - 'A';
        var room = (i - 3) / 2;
        rooms[room] = new amphipod(type);
      }
    }

    static int[] get_solved_rooms(int max) {
      var res = new int[4];

      for (var i = 0; i < 4; i++) {
        var cnt = max;

        for (var j = max - 1; j >= 0; j--) {
          var pad = rooms[j * 4 + i];

          if (pad == amphipod.empty || pad.type == i) {
            if (pad != amphipod.empty)
              cnt--;
          }
          else {
            cnt = -1;
          }
        }

        res[i] = cnt;
      }

      return res;
    }

    static (int, int) path_to_room(int[] solved, int hallway, int room) {
      var from = (solved[room] - 1) * 4 + room;
      var to = solved[room];

      switch (hallway) {
        case 0: {
          if (hallways[1] == amphipod.empty)
            if (room == 0)
              return (from, to + 2);
            else if (hallways[2] == amphipod.empty)
              if (room == 1)
                return (from, to + 4);
              else if (hallways[3] == amphipod.empty)
                if (room == 2)
                  return (from, to + 6);
                else if (hallways[4] == amphipod.empty)
                  return (from, to + 8);
        }
        break;

        case 1: {
          if (room == 0)
            return (from, to + 1);
          else if (hallways[2] == amphipod.empty)
            if (room == 1)
              return (from, to + 3);
            else if (hallways[3] == amphipod.empty)
              if (room == 2)
                return (from, to + 5);
              else if (hallways[4] == amphipod.empty)
                return (from, to + 7);
        }
        break;

        case 2: {
          if (room == 0 || room == 1)
            return (from, to + 1);
          else if (hallways[3] == amphipod.empty)
            if (room == 2)
              return (from, to + 3);
            else if (hallways[4] == amphipod.empty)
              return (from, to + 5);
        }
        break;

        case 3: {
          if (room == 0) {
            if (hallways[2] == amphipod.empty)
              return (from, to + 3);
          }
          else if (room == 1 || room == 2)
            return (from, to + 1);
          else if (hallways[4] == amphipod.empty)
            return (from, to + 3);
        }
        break;

        case 4: {
          if (room == 2 || room == 3)
            return (from, to + 1);
          else if (hallways[3] == amphipod.empty)
            if (room == 1)
              return (from, to + 3);
            else if (hallways[2] == amphipod.empty)
              return (from, to + 5);
        }
        break;

        case 5: {
          if (room == 3)
            return (from, to + 1);
          else if (hallways[4] == amphipod.empty)
            if (room == 2)
              return (from, to + 3);
            else if (hallways[3] == amphipod.empty)
              if (room == 1)
                return (from, to + 5);
              else if (hallways[2] == amphipod.empty)
                return (from, to + 7);
        }
        break;

        case 6: {
          if (hallways[5] == amphipod.empty)
            if (room == 3)
              return (from, to + 2);
            else if (hallways[4] == amphipod.empty)
              if (room == 2)
                return (from, to + 4);
              else if (hallways[3] == amphipod.empty)
                if (room == 1)
                  return (from, to + 6);
                else if (hallways[2] == amphipod.empty)
                  return (from, to + 8);
        }
        break;
      }

      return (-1, -1);
    }

    static void path_to_hallway(List<(int, int, int)> m, int i, int j, int t) {
      var energy = energies[t];
      var from = j * 4 + i;

      switch (i) {
        case 0: {
          if (hallways[1] == amphipod.empty) {
            m.Add((from, 1, (2 + j) * energy));

            if (hallways[0] == amphipod.empty)
              m.Add((from, 0, (3 + j) * energy));
          }

          if (hallways[2] == amphipod.empty) {
            m.Add((from, 2, (2 + j) * energy));

            if (hallways[3] == amphipod.empty) {
              m.Add((from, 3, (4 + j) * energy));

              if (hallways[4] == amphipod.empty) {
                m.Add((from, 4, (6 + j) * energy));

                if (hallways[5] == amphipod.empty) {
                  m.Add((from, 5, (8 + j) * energy));

                  if (hallways[6] == amphipod.empty)
                    m.Add((from, 6, (9 + j) * energy));
                }
              }
            }
          }
        }
        break;

        case 1: {
          if (hallways[2] == amphipod.empty) {
            m.Add((from, 2, (2 + j) * energy));

            if (hallways[1] == amphipod.empty) {
              m.Add((from, 1, (4 + j) * energy));

              if (hallways[0] == amphipod.empty)
                m.Add((from, 0, (5 + j) * energy));
            }
          }

          if (hallways[3] == amphipod.empty) {
            m.Add((from, 3, (2 + j) * energy));

            if (hallways[4] == amphipod.empty) {
              m.Add((from, 4, (4 + j) * energy));

              if (hallways[5] == amphipod.empty) {
                m.Add((from, 5, (6 + j) * energy));

                if (hallways[6] == amphipod.empty)
                  m.Add((from, 6, (7 + j) * energy));
              }
            }
          }
        }
        break;

        case 2: {
          if (hallways[3] == amphipod.empty) {
            m.Add((from, 3, (2 + j) * energy));

            if (hallways[2] == amphipod.empty) {
              m.Add((from, 2, (4 + j) * energy));

              if (hallways[1] == amphipod.empty) {
                m.Add((from, 1, (6 + j) * energy));

                if (hallways[0] == amphipod.empty)
                  m.Add((from, 0, (7 + j) * energy));
              }
            }
          }

          if (hallways[4] == amphipod.empty) {
            m.Add((from, 4, (2 + j) * energy));

            if (hallways[5] == amphipod.empty) {
              m.Add((from, 5, (4 + j) * energy));

              if (hallways[6] == amphipod.empty)
                m.Add((from, 6, (5 + j) * energy));
            }
          }
        }
        break;

        case 3: {
          if (hallways[4] == amphipod.empty) {
            m.Add((from, 4, (2 + j) * energy));

            if (hallways[3] == amphipod.empty) {
              m.Add((from, 3, (4 + j) * energy));

              if (hallways[2] == amphipod.empty) {
                m.Add((from, 2, (6 + j) * energy));

                if (hallways[1] == amphipod.empty) {
                  m.Add((from, 1, (8 + j) * energy));

                  if (hallways[0] == amphipod.empty)
                    m.Add((from, 0, (9 + j) * energy));
                }
              }
            }
          }

          if (hallways[5] == amphipod.empty) {
            m.Add((from, 5, (2 + j) * energy));

            if (hallways[6] == amphipod.empty)
              m.Add((from, 6, (3 + j) * energy));
          }
        }
        break;
      }
    }

    static int get_min_energy() {
      var res = 0;

      for (var i = 0; i < 7; i++) {
        var pad = hallways[i];

        if (pad == amphipod.empty)
          continue;

        switch (i) {
          case 0:
            res += energies[pad.type] * (pad.type * 2 + 3);
            break;

          case 1:
            res += energies[pad.type] * (pad.type * 2 + 2);
            break;

          case 2:
            res += energies[pad.type] * (pad.type == 0 || pad.type == 1 ? 2 : pad.type * 2);
            break;

          case 3:
            res += energies[pad.type] * (pad.type == 1 || pad.type == 2 ? 2 : 4);
            break;

          case 4:
            res += energies[pad.type] * (pad.type == 2 || pad.type == 3 ? 2 : (3 - pad.type) * 2);
            break;

          case 5:
            res += energies[pad.type] * ((3 - pad.type) * 2 + 2);
            break;

          case 6:
            res += energies[pad.type] * ((3 - pad.type) * 2 + 3);
            break;
        }
      }

      return res;
    }

    static List<(int, int, int)> get_moves(int[] solved) {
      List<(int, int, int)> m = new();

      for (int i = 0; i < 7; i++) {
        var pad = hallways[i];

        if (pad == amphipod.empty || solved[pad.type] < 0)
          continue;

        (int pos, int steps) = path_to_room(solved, i, pad.type);

        if (pos < 0)
          continue;

        m.Add((i, pos, -steps * energies[pad.type]));
      }

      if (m.Count > 0)
        return m;

      for (int i = 0; i < 4; i++) {
        if (solved[i] >= 0)
          continue;

        for (int j = 0; j < 4; j++) {
          var pad = rooms[j * 4 + i];

          if (pad == amphipod.empty)
            continue;

          path_to_hallway(m, i, j, pad.type);

          break;
        }
      }

      return m;
    }

    static void run(int energy, int cnt) {
      var solved = get_solved_rooms(cnt);

      if (solved[0] == 0 && solved[1] == 0 && solved[2] == 0 && solved[3] == 0) {
        if (energy < lowest_energy)
          lowest_energy = energy;
        return;
      }

      var min = get_min_energy();

      if (min + energy >= lowest_energy)
        return;

      List<(int, int, int)> moves = get_moves(solved);

      for (int i = 0; i < moves.Count; i++) {
        (int from, int to, int e) = moves[i];
        int next = energy + Math.Abs(e);

        if (next >= lowest_energy)
          continue;

        if (e < 0) {
          rooms[to] = hallways[from];
          hallways[from] = amphipod.empty;
        } else {
          hallways[to] = rooms[from];
          rooms[from] = amphipod.empty;
        }

        run(next, cnt);

        if (e < 0) {
          hallways[from] = rooms[to];
          rooms[to] = amphipod.empty;
        } else {
          rooms[from] = hallways[to];
          hallways[to] = amphipod.empty;
        }
      }
    }

    static void Main(string[] args) {
      var all_lines = File.ReadAllLines("input.txt");
      parse(all_lines);

      lowest_energy = int.MaxValue;
      run(0, 2);
      Console.WriteLine("Least energy required to organize the amphipods (Part 1): " + lowest_energy);

      parse(all_lines);

      for (var i = 4; i < 8; i++) {
        var pad = rooms[i];
        rooms[i + 8] = new amphipod(pad.type);
      }

      rooms[4] = new amphipod(3);
      rooms[5] = new amphipod(2);
      rooms[6] = new amphipod(1);
      rooms[7] = new amphipod(0);

      rooms[8] = new amphipod(3);
      rooms[9] = new amphipod(1);
      rooms[10] = new amphipod(0);
      rooms[11] = new amphipod(2);

      lowest_energy = int.MaxValue;
      run(0, 4);

      Console.WriteLine("Least energy required to organize the amphipods (Part 2): " + lowest_energy);
    }
  }
}
