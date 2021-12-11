#include "../utilities.h"

#define N 10

int octopuses[N * N];
int flashed[N * N];

void parse_input() {
  struct string input = read_file("input.txt");
  char* input_start = input.data;

  for (int row = 0, p = 0; input.size > 0; row++) {
    struct string line = string_split(&input, '\n');
    for (int column = 0; column < N; ++column) {
      octopuses[p++] = line.data[column] - '0';
    }
  }

  free(input_start);
}

void flash(int pos, int x, int y, int* cnt) {
  if (flashed[pos] || ++octopuses[pos] <= 9)
    return;

  flashed[pos] = 1;
  octopuses[pos] = 0;
  (*cnt)++;

  for (int i = -1; i <= 1; i++) {
    int ny = y + i;
    if (ny < 0 || ny >= N)
      continue;

    for (int j = -1; j <= 1; j++) {
      int nx = x + j;

      if (nx >= 0 && nx < N)
        flash(pos + i * N + j, nx, ny, cnt);
    }
  }
}

int step() {
  memset(flashed, 0, sizeof(flashed));

  int count = 0;
  for (int i = 0, p = 0; i < N; i++)
    for (int j = 0; j < N; j++)
      flash(p++, j, i, &count);

  return count;
}

int part1() {
  parse_input();

  int flashes = 0;
  for (int i = 0; i < 100; i++)
    flashes += step();

  return flashes;
}

int part2() {
  parse_input();

  int count = 0;
  while (1) {
    count++;
    if (step() == N * N)
      return count;
  }
}

int main(int argc, char** argv) {
  printf("Flashes after 100 days (Part 1): %d\n", part1());
  printf("First step during which all octopuses flash (Part 2): %d\n", part2());
  system("pause");
}