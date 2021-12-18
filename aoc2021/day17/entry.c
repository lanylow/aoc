#include <stdio.h>
#include <stdlib.h>
#include <math.h>

int coords[4];

int is_between(int val, int min, int max) {
  return (val >= min && val <= max) || (val >= max && val <= min);
}

int sign(int val) {
  return val > 0 ? 1 : val == 0 ? 0 : -1;
}

int validate_vel(int x, int y) {
  int tx = 0, ty = 0;

  while (tx <= coords[1] && ty >= coords[2]) {
    tx += x;
    ty += y;

    x -= sign(x);
    y -= 1;

    if (is_between(tx, coords[0], coords[1]) && is_between(ty, coords[2], coords[3]))
      return 1;
  }

  return 0;
}

int main(int argc, char** argv) {
  FILE* file = fopen("input.txt", "r");
  fscanf(file, "target area: x=%d..%d, y=%d..%d", &coords[0], &coords[1], &coords[2], &coords[3]);
  fclose(file);

  int x_min = sqrt(coords[0] * 2);
  int x_max = coords[1] + 1;

  int y_min = -abs(coords[2]);
  int y_max = abs(coords[2]) - 1;

  int cnt = 0;

  for (int tx = x_min; tx <= x_max; tx++)
    for (int ty = y_min; ty <= y_max; ty++)
      if (validate_vel(tx, ty))
        cnt++;

  printf("Highest y position it reaches on this trajectory (Part 1): %d\n", y_max * (y_max + 1) / 2);
  printf("Distinct initial velocity values that cause the probe to be within the target area after any step (Part 2): %d\n", cnt);
  
  system("pause");
}