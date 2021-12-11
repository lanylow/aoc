#include "../utilities.h"

uint64_t table[9];
uint64_t buffer[9];

void do_next_day() {
  memset(buffer, 0, sizeof(uint64_t) * 9);

  buffer[8] += table[0];
  buffer[6] += table[0];

  for (int i = 1; i < 9; ++i)
    buffer[i - 1] += table[i];

  memcpy(table, buffer, sizeof(uint64_t) * 9);
}

uint64_t get_lanterfish_count() {
  uint64_t res = 0;
  
  for (int i = 0; i < 9; ++i)
    res += table[i];

  return res;
}

int main(int argc, char** argv) {
  struct string input = read_file("input.txt");
  char* input_start = input.data;

  memset(table, 0, sizeof(uint64_t) * 9);

  while (input.size >= 1) {
    struct string split = string_split(&input, ',');
    table[string_to_uint64(split)]++;
  }

  for (int i = 0; i < 80; ++i)
    do_next_day();

  printf("Lanternfishes after 80 days (Part 1): %I64d\n", get_lanterfish_count());

  for (int i = 0; i < 256 - 80; ++i)
    do_next_day();

  printf("Lanternfishes after 256 days (Part 2): %I64d\n", get_lanterfish_count());

  free(input_start);

  system("pause");

  return 0;
}