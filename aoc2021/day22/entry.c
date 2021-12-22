#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include <string.h>

struct cube {
  int64_t x1, x2, y1, y2, z1, z2;
};

struct cube table[1024];
int table_cnt = 0;
int8_t enabled[1024];

void parse() {
  int64_t x1, x2, y1, y2, z1, z2;

  char enable[5];

  FILE* file = fopen("input.txt", "r");

  while (fscanf(file, " %s x=%lld..%lld,y=%lld..%lld,z=%lld..%lld", enable, &x1, &x2, &y1, &y2, &z1, &z2) != -1) {
    table[table_cnt] = (struct cube){ x1, x2, y1, y2, z1, z2 };
    enabled[table_cnt] = enable[1] == 'n';
    table_cnt++;
  }

  fclose(file);
}

int64_t area(struct cube cb) {
  return (cb.x2 - cb.x1 + 1) * (cb.y2 - cb.y1 + 1) * (cb.z2 - cb.z1 + 1);
}

int8_t overlaps(struct cube c1, struct cube c2) {
  return !(c1.x2 < c2.x1 || c1.x1 > c2.x2 || c1.y2 < c2.y1 || c1.y1 > c2.y2 || c1.z2 < c2.z1 || c1.z1 > c2.z2);
}

void substract(struct cube c1, struct cube c2, struct cube* out, int* i) {
  if (!overlaps(c1, c2)) {
    out[(*i)++] = c1;
    return;
  }

  int64_t x1max = max(c1.x1, c2.x1);
  int64_t x2min = min(c1.x2, c2.x2);
  int64_t y1max = max(c1.y1, c2.y1);
  int64_t y2min = min(c1.y2, c2.y2);

  if (c2.x1 > c1.x1)
    out[(*i)++] = (struct cube){ c1.x1, c2.x1 - 1, c1.y1, c1.y2, c1.z1, c1.z2 };

  if (c2.y1 > c1.y1)
    out[(*i)++] = (struct cube){ x1max, x2min, c1.y1, c2.y1 - 1, c1.z1, c1.z2 };

  if (c2.z1 > c1.z1)
    out[(*i)++] = (struct cube){ x1max, x2min, y1max, y2min, c1.z1, c2.z1 - 1 };

  if (c2.x2 < c1.x2)
    out[(*i)++] = (struct cube){ c2.x2 + 1, c1.x2, c1.y1, c1.y2, c1.z1, c1.z2 };

  if (c2.y2 < c1.y2)
    out[(*i)++] = (struct cube){ x1max, x2min, c2.y2 + 1, c1.y2, c1.z1, c1.z2 };

  if (c2.z2 < c1.z2)
    out[(*i)++] = (struct cube){ x1max, x2min, y1max, y2min, c2.z2 + 1, c1.z2 };
}

int64_t get_result(int8_t in_bounds) {
  static struct cube buf[1024 * 1024];
  int buf_cnt = 0;

  static struct cube buf1[1024 * 1024];
  int buf1_cnt = 0;
  
  for (int i = 0; i < table_cnt; i++) {
    static const struct cube bounds = { -50, 50, -50, 50, -50, 50 };

    if (in_bounds && !overlaps(table[i], bounds))
      continue;

    buf_cnt = 0;

    for (int j = 0; j < buf1_cnt; j++)
      substract(buf1[j], table[i], buf, &buf_cnt);

    if (enabled[i])
      buf[buf_cnt++] = table[i];

    memcpy(buf1, buf, buf_cnt * sizeof(struct cube));
    buf1_cnt = buf_cnt;
  }

  int64_t res = 0;

  for (int i = 0; i < buf1_cnt; i++)
    res += area(buf1[i]);

  return res;
}

int main(int argc, char** argv) {
  parse();

  printf("Amount of enabled cubes within given bounds (Part 1): %I64d\n", get_result(1));
  printf("Amount of enabled cubes (Part 2): %I64d\n", get_result(0));

  return 0;
}