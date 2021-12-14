#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <stdint.h>

char table[128][128];
char input[64];
int64_t counts[128];
int64_t table_cnt[128][128];
int64_t buf[128][128];

void insert(char i, char j, int cnt) {
  if (!cnt)
    return;

  char n = table[i][j];
  counts[n]++;

  insert(i, n, cnt - 1);
  insert(n, j, cnt - 1);
}

int compare(const void* i, const void* j) {
  int64_t res = *(int64_t*)i - *(int64_t*)j;
  return res < 0 ? 1 : res > 0 ? -1 : 0;
}

int64_t get_result() {
  qsort(counts, 128, sizeof(int64_t), compare);

  int64_t res = 0;
  for (int i = 0; i < 128; i++)
    res = counts[i] ? counts[i] : res;

  return counts[0] - res;
}

void part1() {
  for (int i = 0; input[i + 1]; i++)
    insert(input[i], input[i + 1], 10);

  for (int i = 0; input[i]; i++)
    counts[input[i]]++;

  printf("Quantity of the most common element - quantity of the least common element (Part 1): %I64d\n", get_result());
}

void part2() {
  memset(table_cnt, 0, sizeof(table_cnt));

  for (int i = 0; input[i]; i++) {
    table_cnt[input[i - 1]][input[i]]++;
    counts[input[i]]++;
  }

  for (int i = 40; i; i--) {
    memset(buf, 0, sizeof(buf));

    for (char j = 'A'; j <= 'Z'; j++) {
      for (char k = 'A'; k <= 'Z'; k++) {
        char l = table[j][k];

        if (l) {
          buf[j][l] += table_cnt[j][k];
          counts[l] += table_cnt[j][k];
          buf[l][k] += table_cnt[j][k];
        }
        else {
          buf[j][k] += table_cnt[j][k];
        }
      }
    }

    memcpy(table_cnt, buf, sizeof(buf));
  }

  printf("Quantity of the most common element - quantity of the least common element (Part 2): %I64d\n", get_result());
}

int main(int argc, char** argv) {
  FILE* file = fopen("input.txt", "r");
  if (!file)
    return 1;

  fscanf(file, "%[^\n] ", input);

  char i, j, k;
  while (fscanf(file, "%c%c -> %c\n", &i, &j, &k) != -1)
    table[i][j] = k;

  fclose(file);

  // part1();
  part2();

  system("pause");
}