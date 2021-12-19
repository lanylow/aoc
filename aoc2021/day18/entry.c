#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

struct sfn {
  int val;
  int dep;
};

struct sfna {
  struct sfn n[128];
  int cnt;
};

struct sfna table[128];
struct sfna cur;
int lines = 0;

char* fgetstripped(char* buf, int cnt, FILE* file) {
  char* tmp = fgets(buf, cnt, file);

  if (tmp && tmp[strlen(tmp) - 1] == '\n')
    tmp[strlen(tmp) - 1] = '\0';

  return tmp;
}

void parse() {
  FILE* file = fopen("input.txt", "r");
  char buf[2048];

  while (fgetstripped(buf, 2048, file)) {
    char* tmp = buf;
    int cnt = 0;
    int dep = 0;

    while (*tmp) {
      if (isdigit(*tmp)) {
        table[lines].n[cnt].val = strtol(tmp, &tmp, 10);
        table[lines].n[cnt].dep = dep;
        cnt++;
      }
      else {
        if (*tmp == '[')
          dep++;

        if (*tmp == ']')
          dep--;

        tmp++;
      }
    }

    table[lines].cnt = cnt;
    lines++;
  }

  fclose(file);
}

void shiftl(struct sfna* arr, int idx) {
  for (int i = idx; i < arr->cnt; i++)
    arr->n[i - 1] = arr->n[i];
  arr->cnt--;
}

void shiftr(struct sfna* arr, int idx) {
  for (int i = arr->cnt; i > idx; i--)
    arr->n[i] = arr->n[i - 1];
  arr->cnt++;
}

int split(struct sfna* arr) {
  for (int i = 0; i < arr->cnt; i++) {
    if (arr->n[i].val >= 10) {
      struct sfn n = arr->n[i];
      shiftr(arr, i);
      arr->n[i] = (struct sfn){ n.val / 2, n.dep + 1 };
      arr->n[i + 1] = (struct sfn){ (n.val + 1) / 2, n.dep + 1 };
      return 1;
    }
  }

  return 0;
}

int explode(struct sfna* arr) {
  for (int i = 0; i < arr->cnt - 1; i++) {
    if (arr->n[i].dep == arr->n[i + 1].dep && arr->n[i].dep > 4) {
      if (i > 0)
        arr->n[i - 1].val += arr->n[i].val;

      if (i < arr->cnt - 2)
        arr->n[i + 2].val += arr->n[i + 1].val;

      shiftl(arr, i + 2);
      arr->n[i].val = 0;
      arr->n[i].dep--;
      return 1;
    }
  }

  return 0;
}

struct sfna* concat(struct sfna* l, struct sfna* r) {
  for (int i = 0; i < l->cnt; i++)
    l->n[i].dep++;

  for (int i = 0; i < r->cnt; i++) {
    l->n[l->cnt] = r->n[i];
    l->n[l->cnt].dep++;
    l->cnt++;
  }

  while (1) {
    if (explode(l))
      continue;

    if (split(l))
      continue;

    break;
  }

  return l;
}

int get_magnitude(struct sfna* arr) {
  struct sfna tmp = *arr;

  while (tmp.cnt > 1) {
    for (int i = 0; i < arr->cnt - 1; i++) {
      if (tmp.n[i].dep == tmp.n[i + 1].dep) {
        tmp.n[i].val = tmp.n[i].val * 3 + tmp.n[i + 1].val * 2;
        tmp.n[i].dep--;
        shiftl(&tmp, i + 2);
        break;
      }
    }
  }

  return tmp.n[0].val;
}

void part1() {
  cur = table[0];
  
  for (int i = 1; i < lines; i++)
    concat(&cur, &table[i]);

  printf("Magnitude of the final sum (Part 1): %d\n", get_magnitude(&cur));
}

void part2() {
  int res = 0;
  int tmp = 0;

  for (int i = 0; i < lines; i++) {
    for (int j = 0; j < lines; j++) {
      if (i == j)
        continue;

      cur = table[i];
      tmp = get_magnitude(concat(&cur, &table[j]));
      res = max(res, tmp);

      cur = table[j];
      tmp = get_magnitude(concat(&cur, &table[i]));
      res = max(res, tmp);
    }
  }

  printf("Largest magnitude of any sum of two different snailfish numbers (Part 2): %d\n", res);
}

int main(int argc, char** argv) {
  parse();
  part1();
  part2();

  system("pause");
}