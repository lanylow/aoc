import java.io.*;
import java.util.*;

public class Main {
  private static class Vector3D {
    public final int x, y, z;

    public Vector3D(int _x, int _y, int _z) {
      x = _x;
      y = _y;
      z = _z;
    }
  }

  private static class Vector4D {
    public final int x, y, z, w;

    public Vector4D(int _x, int _y, int _z, int _w) {
      x = _x;
      y = _y;
      z = _z;
      w = _w;
    }
  }

  private static class Cube3D {
    public final Vector3D coord;
    public boolean active;

    public Cube3D(Vector3D _coord, boolean _active) {
      coord = _coord;
      active = _active;
    }
  }

  private static class Cube4D {
    public final Vector4D coord;
    public boolean active;

    public Cube4D(Vector4D _coord, boolean _active) {
      coord = _coord;
      active = _active;
    }
  }

  private static List<String> parse() throws IOException {
    List<String> res = new ArrayList<>();
    var bufferedReader = new BufferedReader(new FileReader("input.txt"));

    var line = bufferedReader.readLine();

    while (line != null) {
      res.add(line);
      line = bufferedReader.readLine();
    }

    bufferedReader.close();

    return res;
  }

  private static boolean isInRange(int v, int max) {
    return v >= 0 && v <= max;
  }

  private static int getActiveAdjacent3D(Cube3D[][][] cubes, Cube3D cube) {
    var coords = cube.coord;
    var res = 0;

    for (var i = -1; i <= 1; i++) {
      for (var j = -1; j <= 1; j++) {
        for (var k = -1; k <= 1; k++) {
          if (!(i == 0 && j == 0 && k == 0)) {
            var x = coords.x + i;
            var y = coords.y + j;
            var z = coords.z + k;

            if (isInRange(x, cubes.length - 1) && isInRange(y, cubes.length - 1) && isInRange(z, cubes.length - 1))
              if (cubes[x][y][z].active)
                res++;
          }
        }
      }
    }

    return res;
  }

  private static int getActiveAdjacent4D(Cube4D[][][][] cubes, Cube4D cube) {
    var coords = cube.coord;
    var res = 0;

    for (var i = -1; i <= 1; i++) {
      for (var j = -1; j <= 1; j++) {
        for (var k = -1; k <= 1; k++) {
          for (var l = -1; l <= 1; l++) {
            if (!(i == 0 && j == 0 && k == 0 && l == 0)) {
              var x = coords.x + i;
              var y = coords.y + j;
              var z = coords.z + k;
              var w = coords.w + l;

              if (isInRange(x, cubes.length - 1) && isInRange(y, cubes.length - 1) && isInRange(z, cubes.length - 1) && isInRange(w, cubes.length - 1))
                if (cubes[x][y][z][w].active)
                  res++;
            }
          }
        }
      }
    }

    return res;
  }

  private static Cube3D[][][] simulate3D(Cube3D[][][] cubes) {
    var newCubes = new Cube3D[cubes.length][cubes[0].length][cubes[0][0].length];

    for (var i = 0; i < cubes.length; i++) {
      for (var j = 0; j < cubes[i].length; j++) {
        for (var k = 0; k < cubes[i][j].length; k++) {
          var cube = cubes[i][j][k];
          var cnt = getActiveAdjacent3D(cubes, cube);

          if (cube.active && (cnt < 2 || cnt > 3)) {
            newCubes[i][j][k] = new Cube3D(new Vector3D(i, j, k), false);
            continue;
          } else if (!cube.active && cnt == 3) {
            newCubes[i][j][k] = new Cube3D(new Vector3D(i, j, k), true);
            continue;
          }

          newCubes[i][j][k] = cubes[i][j][k];
        }
      }
    }

    return newCubes;
  }

  private static Cube4D[][][][] simulate4D(Cube4D[][][][] cubes) {
    var newCubes = new Cube4D[cubes.length][cubes[0].length][cubes[0][0].length][cubes[0][0][0].length];

    for (var i = 0; i < cubes.length; i++) {
      for (var j = 0; j < cubes[i].length; j++) {
        for (var k = 0; k < cubes[i][j].length; k++) {
          for (var l = 0; l < cubes[i][j][k].length; l++) {
            var cube = cubes[i][j][k][l];
            var cnt = getActiveAdjacent4D(cubes, cube);

            if (cube.active && (cnt < 2 || cnt > 3)) {
              newCubes[i][j][k][l] = new Cube4D(new Vector4D(i, j, k, l), false);
              continue;
            } else if (!cube.active && cnt == 3) {
              newCubes[i][j][k][l] = new Cube4D(new Vector4D(i, j, k, l), true);
              continue;
            }

            newCubes[i][j][k][l] = cubes[i][j][k][l];
          }
        }
      }
    }

    return newCubes;
  }

  private static int part1(List<String> input) {
    var len = input.size() + 6 * 2;
    var cubes = new Cube3D[len][len][len];

    for (var i = 0; i < cubes.length; i++)
      for (var j = 0; j < cubes[0].length; j++)
        for (var k = 0; k < cubes[0][0].length; k++)
          cubes[i][j][k] = new Cube3D(new Vector3D(i, j, k), false);

    for (int idx = 0, i = cubes.length / 2 + 1, j = 6; idx < input.size(); idx++, j++) {
      var s = input.get(idx);

      for (int c = 0, k = 6; c < s.length(); c++, k++)
        cubes[i][j][k] = new Cube3D(new Vector3D(i, j, k), s.charAt(c) == '#');
    }

    for (var i = 0; i < 6; i++)
      cubes = simulate3D(cubes);

    var res = 0;

    for (var x : cubes)
      for (var y : x)
        for (var z : y)
          if (z.active)
            res++;

    return res;
  }

  private static int part2(List<String> input) {
    var len = input.size() + 6 * 2;
    var cubes = new Cube4D[len][len][len][len];

    for (var i = 0; i < cubes.length; i++)
      for (var j = 0; j < cubes[i].length; j++)
        for (var k = 0; k < cubes[i][j].length; k++)
          for (var l = 0; l < cubes[i][j][k].length; l++)
            cubes[i][j][k][l] = new Cube4D(new Vector4D(i, j, k, l), false);

    for (int idx = 0, i = cubes.length / 2 + 1, j = 6; idx < input.size(); idx++, j++) {
      var s = input.get(idx);

      for (int c = 0, k = 6; c < s.length(); c++, k++)
        cubes[i][i][j][k] = new Cube4D(new Vector4D(i, i, j, k), s.charAt(c) == '#');
    }

    for (var i = 0; i < 6; i++)
      cubes = simulate4D(cubes);

    var res = 0;

    for (var x : cubes)
      for (var y : x)
        for (var z : y)
          for (var w : z)
            if (w.active)
              res++;

    return res;
  }

  public static void main(String[] args) {
    try {
      var input = parse();

      System.out.println("Cubes left in the active state after the sixth cycle (Part 1): " + part1(input));
      System.out.println("Cubes left in the active state after the sixth cycle (Part 2): " + part2(input));
    } catch (IOException e) {
      System.out.println(e.getMessage());
    }
  }
}