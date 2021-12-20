#include <stdlib.h>
#include <stdio.h>
#include <string.h>

int table[256][256];
int table_cnt;
int buff[256][256];
int map[256] = { 0 };
int def = 0;

char* fgetstripped(char* buf, int cnt, FILE* file) {
  char* tmp = fgets(buf, cnt, file);

  if (tmp && tmp[strlen(tmp) - 1] == '\n')
    tmp[strlen(tmp) - 1] = '\0';

  return tmp;
}

void parse() {
  FILE* file = fopen("input.txt", "r");
  char buf[2048];

  fgetstripped(buf, 2048, file);

  int i = 0;
  for (char* l = buf; *l; l++)
    map[i++] = (*l) == '#';

  fgetstripped(buf, 2048, file);

  while (fgetstripped(buf, 2048, file)) {
    int i = 0;
    for (char* l = buf; *l; l++)
      table[table_cnt][i++] = (*l) == '#';
    table_cnt++;
  }

  fclose(file);
}

int at(int x, int y) {
  if (x < 0 || x >= table_cnt || y < 0 || y >= table_cnt)
    return def;

  return table[x][y];
}

int map_idx(int x, int y) {
  static int near[][2] = {
    { -1, -1 },
    { -1, 0 },
    { -1, 1 },
    {  0, -1 },
    {  0, 0 },
    {  0, 1 },
    {  1, -1 },
    {  1, 0 },
    {  1, 1 }
  };

  int res = 0;

  for (int i = 0; i < 9; i++)
    res = res << 1 | at(x + near[i][0], y + near[i][1]);

  return res;
}

int enhance() {
  int res = 0;

  for (int x = -1; x <= table_cnt; x++) {
    for (int y = -1; y <= table_cnt; y++) {
      buff[x + 1][y + 1] = map[map_idx(x, y)];
      res += buff[x + 1][y + 1];
    }
  }

  table_cnt += 2;
  memcpy(table, buff, sizeof(table));

  if (map[0] == 1)
    def = !def;

  return res;
}

int main(int argc, char** argv) {
  parse();

  int i = 0, res;

  for (; i < 2; i++)
    res = enhance();

  printf("Lit pixels after applying the algorithm twice: %d\n", res);

  for (; i < 50; i++)
    res = enhance();

  printf("Lit pixels after applying the algorithm 50 times: %d\n", res);

  system("pause");
}