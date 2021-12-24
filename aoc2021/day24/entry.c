#include <stdlib.h>
#include <stdio.h>
#include <string.h>

int table[14][3];

char* fgetstripped(char* buf, int cnt, FILE* file) {
  char* tmp = fgets(buf, cnt, file);

  if (tmp && tmp[strlen(tmp) - 1] == '\n')
    tmp[strlen(tmp) - 1] = '\0';

  return tmp;
}

void parse() {
  FILE* file = fopen("input.txt", "r");
  char buf[2048];

  int b = -1, l = 0;

  while (fgetstripped(buf, sizeof(buf), file)) {
    if (strncmp(buf, "inp", 3) == 0) {
      b++;
      l = 0;
    }
    else {
      l++;

      if (l == 4)
        table[b][0] = buf[strlen(buf) - 1] == '1';
      else if (l == 5)
        sscanf(buf, "add x %d", &table[b][1]);
      else if (l == 15)
        sscanf(buf, "add y %d", &table[b][2]);
    }
  }

  fclose(file);
}

int get_result(int largest, int i, int z, char* res) {
  if (i == 14 && z == 0)
    return 1;

  if (table[i][0]) {
    for (int it = 1; it < 10; it++) {
      int j = largest ? 10 - it : it;
      int nz = 26 * z + table[i][2] + j;

      if (get_result(largest, i + 1, nz, res)) {
        res[i] = j + '0';
        return 1;
      }
    }
  }
  else {
    int d = (z % 26) + table[i][1];

    if (1 <= d && d <= 9 && get_result(largest, i + 1, z / 26, res)) {
      res[i] = d + '0';
      return 1;
    }
  }

  return 0;
}

void part1() {
  char res[15] = { 0 };
  get_result(1, 0, 0, res);
  printf("Largest model number accepted by MONAD (Part 1): %s\n", res);
}

void part2() {
  char res[15] = { 0 };
  get_result(0, 0, 0, res);
  printf("Smallest model number accepted by MONAD (Part 1): %s\n", res);
}

int main(int argc, char** argv) {
  parse();
  part1();
  part2();

  return 0;
}