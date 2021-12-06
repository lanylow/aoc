#include <stdio.h>
#include <stdint.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

struct string {
  size_t size;
  char* data;
};

struct string string_split(struct string* in, char delim) {
  uint64_t i = 0;

  while (i < in->size && in->data[i] != delim)
    i += 1;

  struct string result;
  result.size = i;
  result.data = in->data;

  if (i < in->size) {
    in->size -= i + 1;
    in->data += i + 1;
  }
  else {
    in->size -= i;
    in->data += i;
  }

  return result;
}

uint64_t string_to_uint64(struct string in) {
  uint64_t res = 0;

  for (uint64_t i = 0; i < in.size && isdigit(in.data[i]); i++)
    res = res * 10 + (uint64_t)in.data[i] - '0';

  return res;
}

uint64_t table[9];
uint64_t buffer[9];

struct string read_file(const char* name) {
  struct string out;
  FILE* file;
  fopen_s(&file, name, "rb");
  fseek(file, 0, SEEK_END);
  out.size = ftell(file);
  fseek(file, 0, SEEK_SET);

  out.data = malloc(out.size + 1);
  fread(out.data, out.size, 1, file);
  fclose(file);
  out.data[out.size] = 0;

  return out;
}

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

  system("pause");

  return 0;
}