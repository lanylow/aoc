#include <stdlib.h>
#include <stdio.h>
#include <string.h>

struct table {
  int table[2048][2048];
  int width, height;
};

struct fold {
  int direction;
  int position;
};

struct table input = { 0 };
struct fold folds[64] = { 0 };
int folds_cnt = 0;

void fold(struct table* state, struct fold instruction) {
  if (!instruction.direction) {
    for (int i = 0; i < instruction.position; i++)
      for (int j = 0; j < state->width; j++)
        if (state->table[instruction.position * 2 - i][j])
          state->table[i][j] = 1;

    state->height = instruction.position;
    return;
  }

  for (int i = 0; i < state->width; i++)
    for (int j = 0; j < instruction.position; j++)
      if (state->table[i][instruction.position * 2 - j])
        state->table[i][j] = 1;

  state->width = instruction.position;
}

void part1() {
  struct table* state = malloc(sizeof(struct table));
  if (!state)
    return;

  memcpy(state, &input, sizeof(struct table));
  fold(state, folds[0]);

  int res = 0;
  for (int i = 0; i < state->height; i++)
    for (int j = 0; j < state->width; j++)
      if (state->table[i][j])
        res++;

  printf("Dots visible after completing just the first fold (Part 1): %d\n", res);

  free(state);
}

void part2() {
  struct table* state = malloc(sizeof(struct table));
  if (!state)
    return;

  memcpy(state, &input, sizeof(struct table));

  for (int i = 0; i < folds_cnt; i++)
    fold(state, folds[i]);

  printf("Code used to activate the infrared thermal imaging camera system (Part 2):\n");

  for (int i = 0; i < state->height; i++) {
    for (int j = 0; j < state->width; j++)
      printf(state->table[i][j] ? "#" : " ");
    printf("\n");
  }

  free(state);
}

int main(int argc, char** argv) {
  FILE* file = fopen("input.txt", "r");

  if (!file)
    return 1;

  input.width = 2048;
  input.height = 2048;

  int x, y;
  while (fscanf(file, "%d,%d", &x, &y) == 2)
    input.table[y][x] = 1;

  char direction;
  int position;
  while (fscanf(file, "fold along %c=%d", &direction, &position) == 2) {
    folds[folds_cnt].direction = direction == 'x';
    folds[folds_cnt++].position = position;
    fgetc(file);
  }

  part1();
  part2();

  system("pause");
  fclose(file);
}