#include <stdlib.h>
#include <stdio.h>
#include <string.h>

char* map, *next_map;
int width, height;

char* fgetstripped(char* buf, int cnt, FILE* file) {
  char* tmp = fgets(buf, cnt, file);

  if (tmp && tmp[strlen(tmp) - 1] == '\n')
    tmp[strlen(tmp) - 1] = '\0';

  return tmp;
}

void parse() {
  FILE* file = fopen("input.txt", "r");
  char buf[2048];
  int pos = 0;

  while (fgetstripped(buf, sizeof(buf), file)) {
    if (!width) {
      width = strlen(buf);

      map = malloc(width * width);
      next_map = malloc(width * width);
    }

    for (int i = 0; i < width; i++)
      map[pos++] = buf[i];

    height++;
  }

  fclose(file);
}

int run() {
  int moves = 0;

  memcpy(next_map, map, width * width);

  for (int i = 0, pos = 0; i < height; i++) {
    for (int j = 0; j < width; j++, pos++) {
      char v = map[pos];

      if (v != '>')
        continue;

      int next_pos = j + 1 < width ? pos + 1 : pos + 1 - width;

      if (map[next_pos] == '.') {
        next_map[pos] = '.';
        next_map[next_pos] = '>';
        moves++;
      }
    }
  }

  memcpy(map, next_map, width * width);

  for (int i = 0, pos = 0; i < height; i++) {
    for (int j = 0; j < width; j++, pos++) {
      char v = next_map[pos];

      if (v != 'v')
        continue;

      int next_pos = i + 1 < height ? pos + width : pos % width;

      if (next_map[next_pos] == '.') {
        map[pos] = '.';
        map[next_pos] = 'v';
        moves++;
      }
    }
  }

  return moves;
}

int main(int argc, char** argv) {
  parse();

  int steps = 1;

  while (run() > 0)
    steps++;

  printf("First step on which no sea cucumbers move: %d\n", steps);
}