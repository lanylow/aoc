#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <stdint.h>

int8_t table[512][512];
int risks[1024][1024];
int width, height;

int parse_input() {
  FILE* file = fopen("input.txt", "r");
  if (!file)
    return 1;

  char buf[1024];
  while (fscanf(file, "%[^\n] ", buf) != -1) {
    width = strlen(buf);

    for (int i = 0; i < width; i++)
      table[height][i] = buf[i] - '0';

    height++;
  }

  fclose(file);

  return 0;
}

int get_path(int length) {
  int stop = 0;

  for (int i = 0; i < height * length; i++) {
    for (int j = 0; j < width * length; j++) {
      risks[i][j] = INT_MAX - 10;
      table[i][j] = (table[i % height][j % width] + j / width + i / height - 1) % 9 + 1;
    }
  }

  risks[0][0] = 0;

  do {
    stop = 1;

    for (int i = 0; i < height * length; i++) {
      for (int j = 0; j < width * length; j++) {
        int risk = risks[i][j];

        if (i)
          risk = min(risk, risks[i - 1][j] + table[i][j]);

        if (j)
          risk = min(risk, risks[i][j - 1] + table[i][j]);

        if (i < height * length - 1)
          risk = min(risk, risks[i + 1][j] + table[i][j]);

        if (j < width * length - 1)
          risk = min(risk, risks[i][j + 1] + table[i][j]);

        if (risk != risks[i][j]) {
          risks[i][j] = risk;
          stop = 0;
        }
      }
    }
  } while (!stop);

  return risks[height * length - 1][width * length - 1];
}

int main(int argc, char** argv) {
  parse_input();

  printf("Lowest total risk of any path from the top left to the bottom right (Part 1): %d\n", get_path(1));
  printf("Lowest total risk of any path from the top left to the bottom right (Part 2): %d\n", get_path(5));

  system("pause");
}