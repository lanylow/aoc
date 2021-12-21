#include <stdlib.h>
#include <stdio.h>
#include <stdint.h>
#include <string.h>

int p1s, p2s;

void swap(uint64_t* l, uint64_t* r) {
  uint64_t t = *l;
  *l = *r;
  *r = t;
}

char* fgetstripped(char* buf, int cnt, FILE* file) {
  char* tmp = fgets(buf, cnt, file);

  if (tmp && tmp[strlen(tmp) - 1] == '\n')
    tmp[strlen(tmp) - 1] = '\0';

  return tmp;
}

void parse() {
  FILE* file = fopen("input.txt", "r");
  int tmp;

  fscanf(file, " Player %d starting position: %d", &tmp, &p1s);
  fscanf(file, " Player %d starting position: %d", &tmp, &p2s);

  p1s--;
  p2s--;

  fclose(file);
}

void part1() {
  uint64_t p1 = p1s, p2 = p2s, p1score = 0, p2score = 0, i = 0;

  while (p1score < 1000 && p2score < 1000) {
    uint64_t dice = i % 100 + (i + 1) % 100 + (i + 2) % 100 + 3;
    i += 3;
    p1 = (p1 + dice) % 10;
    p1score += p1 + 1;
    swap(&p1, &p2);
    swap(&p1score, &p2score);
  }

  uint64_t res = min(p1score, p2score) * i;
  printf("Score of the losing player multiplied by the number of times the die was rolled during the game (Part 1): %I64d\n", res);
}

int64_t table[10][10][25][25][2];

void count_wins(uint64_t p1, uint64_t p2, uint64_t p1score, uint64_t p2score, uint64_t* _p1wins, uint64_t* _p2wins) {
  if (table[p1][p2][p1score][p2score][0] != -1) {
    *_p1wins = table[p1][p2][p1score][p2score][0];
    *_p2wins = table[p1][p2][p1score][p2score][1];
    return;
  }

  if (p1score >= 21 || p2score >= 21) {
    *_p1wins = p1score > p2score;
    *_p2wins = p2score > p1score;
    return;
  }

  uint64_t p1wins = 0, p2wins = 0;

  for (int i = 1; i <= 3; i++) {
    for (int j = 1; j <= 3; j++) {
      for (int k = 1; k <= 3; k++) {
        uint64_t tp1 = (p1 + i + j + k) % 10;
        uint64_t rp1, rp2;

        count_wins(p2, tp1, p2score, p1score + tp1 + 1, &rp2, &rp1);

        p1wins += rp1;
        p2wins += rp2;
      }
    }
  }

  table[p1][p2][p1score][p2score][0] = p1wins;
  table[p1][p2][p1score][p2score][1] = p2wins;

  *_p1wins = p1wins;
  *_p2wins = p2wins;
}

void part2() {
  memset(table, -1, sizeof(table));
  uint64_t p1wins, p2wins;
  count_wins(p1s, p2s, 0, 0, &p1wins, &p2wins);
  printf("In how many universes played that won the most win (Part 2): %I64d\n", max(p1wins, p2wins));
}

int main(int argc, char** argv) {
  parse();
  part1();
  part2();
}
