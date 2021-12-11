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
  while (i < in->size && in->data[i] != delim) i++;

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