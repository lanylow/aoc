#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>
#include <math.h>
#include <stdint.h>

#define TILE_SIZE 10

#define ORIENTATION_TOP 0
#define ORIENTATION_RIGHT 1
#define ORIENTATION_BOTTOM 2
#define ORIENTATION_LEFT 3

#define MATCHED_FAILED 0
#define MATCHED_NORMAL 1
#define MATCHED_REVERSED 2

#define REVERSE_Y 0
#define REVERSE_X 1

#define MONSTER_WIDTH 20
#define MONSTER_HEIGHT 3

#define MONSTER \
"" \
"                  # \n" \
"#    ##    ##    ###\n" \
" #  #  #  #  #  #   "

struct tile {
  uint64_t id;
  int x;
  int y;
  char data[TILE_SIZE * TILE_SIZE];
};

int read_file(const char* name, char** out) {
  FILE* file;
  fopen_s(&file, name, "rb");
  if (file == NULL) return 1;

  fseek(file, 0, SEEK_END);
  size_t size = ftell(file);
  fseek(file, 0, SEEK_SET);

  *out = malloc(size + 1);
  if (*out == NULL) return 1;

  fread(*out, size, 1, file);
  fclose(file);
  (*out)[size] = 0;

  return 0;
}

int parse_input(struct tile* out, int* count) {
  char* input = NULL;
  if (read_file("input.txt", &input)) return 1;

  int tile_count = 0;
  char* iter = input;

  while (*iter != '\0') {
    struct tile* tile = &out[tile_count++];

    tile->x = -1;
    tile->y = -1;

    iter += 5;
    tile->id = 0;

    while (isdigit(*iter)) {
      tile->id = (tile->id * 10) + (*iter - '0');
      iter++;
    }

    iter += 3;

    int y = 0;

    while (*iter != '\0') {
      for (int x = 0; *iter != '\n' && *iter != '\r' && *iter != '\0'; iter++, x++) {
        tile->data[y * TILE_SIZE + x] = *iter;
      }

      if (*iter == '\r') {
        iter += 2;
        y++;
      }

      if (*iter == '\r') {
        iter += 2;
        break;
      }
    }
  }

  *count = tile_count;

  return 0;
}

int is_known(struct tile** image, int size, struct tile* tile, int side) {
  switch (side) {
    case ORIENTATION_TOP: {
      return tile->y > 0 ? image[(tile->y - 1) * size + tile->x] != NULL : 0;
    }

    case ORIENTATION_RIGHT: {
      return tile->x < size - 1 ? image[tile->y * size + tile->x + 1] != NULL : 1;
    }

    case ORIENTATION_BOTTOM: {
      return tile->y < size - 1 ? image[(tile->y + 1) * size + tile->x] != NULL : 1;
    }

    case ORIENTATION_LEFT: {
      return tile->x > 0 ? image[tile->y * size + tile->x - 1] != NULL : 0;
    }
  }

  return 0;
}

int get_side(struct tile* tile, int orientation, char* out, int reverse) {
  switch (orientation) {
    case ORIENTATION_TOP: {
      for (int y = 0, x = 0; x < TILE_SIZE; x++) {
        out[x] = tile->data[y * TILE_SIZE + x];
      }

      break;
    }

    case ORIENTATION_RIGHT: {
      for (int y = 0, x = TILE_SIZE - 1; y < TILE_SIZE; y++) {
        out[y] = tile->data[y * TILE_SIZE + x];
      }

      break;
    }

    case ORIENTATION_BOTTOM: {
      for (int y = TILE_SIZE - 1, x = 0; x < TILE_SIZE; x++) {
        out[x] = tile->data[y * TILE_SIZE + x];
      }

      break;
    }

    case ORIENTATION_LEFT: {
      for (int y = 0, x = 0; y < TILE_SIZE; y++) {
        out[y] = tile->data[y * TILE_SIZE + x];
      }

      break;
    }

    default: {
      return 1;
    }
  }

  if (reverse) {
    char c;

    for (int i = 0, j = TILE_SIZE - 1; i < j; i++, j--) {
      c = out[i];
      out[i] = out[j];
      out[j] = c;
    }
  }

  return 0;
}

int compare_sides(char* first, char* second) {
  if (memcmp(first, second, TILE_SIZE) == 0) return MATCHED_NORMAL;

  int i;

  for (i = 0; i < TILE_SIZE; i++) {
    if (first[i] != second[TILE_SIZE - 1 - i]) {
      break;
    }
  }

  if (i == TILE_SIZE) return MATCHED_REVERSED;

  return MATCHED_FAILED;
}

void rotate_tile(char* data, int size) {
  char* out = (char*)malloc(size * size);
  if (out == NULL) return;
  memset(out, 0, size * size);

  for (int y = 0, x1 = size - 1; y < size; y++, x1--) {
    for (int x = 0, y1 = 0; x < size; x++, y1++) {
      out[y1 * size + x1] = data[y * size + x];
    }
  }

  memcpy(data, out, size * size);
  free(out);
}

void reverse_tile(char* data, int size, int side) {
  char* out = (char*)malloc(size * size);
  if (out == NULL) return;
  memset(out, 0, size * size);

  for (int y = 0; y < size; y++) {
    for (int x = 0; x < size; x++) {
      switch (side) {
        case REVERSE_X: {
          out[y * size + (size - 1 - x)] = data[y * size + x];
          break;
        }

        case REVERSE_Y: {
          out[(size - 1 - y) * size + x] = data[y * size + x];
          break;
        }
      }
    }
  }

  memcpy(data, out, size * size);
  free(out);
}

int are_adjacent(struct tile* first, struct tile* second, int orientation) {
  char first_side[TILE_SIZE];
  char second_side[TILE_SIZE];
  int wanted_side = (orientation + 2) % 4;

  get_side(first, orientation, first_side, 0);

  for (int side = ORIENTATION_TOP; side <= ORIENTATION_LEFT; side++) {
    get_side(second, side, second_side, (wanted_side / 2) != (side / 2));
    int matched = compare_sides(first_side, second_side);

    if (matched > 0) {
      while (side != wanted_side) {
        rotate_tile(second->data, TILE_SIZE);
        side = (side + 1) % 4;
      }

      if (matched == MATCHED_REVERSED) {
        reverse_tile(second->data, TILE_SIZE, wanted_side == ORIENTATION_TOP || wanted_side == ORIENTATION_BOTTOM ? REVERSE_X : REVERSE_Y);
      }

      return 1;
    }
  }

  return 0;
}

void move_image(struct tile** image, int size, int delta_x, int delta_y) {
  struct tile** out = (struct tile**)calloc(size * size, sizeof(struct tile*));
  if (out == NULL) return;

  memset(out, 0, size * size * sizeof(struct tile*));
  struct tile* tile;

  for (int y = 0; y < size - delta_y; y++) {
    for (int x = 0; x < size - delta_x; x++) {
      int y1 = y + delta_y;
      int x1 = x + delta_x;

      tile = image[y * size + x];

      if (tile) {
        tile->x = x1;
        tile->y = y1;
        out[y1 * size + x1] = tile;
      }
    }
  }

  memcpy(image, out, size * size * sizeof(struct tile*));
  free(out);
}

void show_image(struct tile** image, int size) {
  for (int iy = 0; iy < size; iy++) {
    for (int y = 0; y < TILE_SIZE; y++) {
      for (int ix = 0; ix < size; ix++) {
        struct tile* tile = image[iy * size + ix];

        for (int x = 0; x < TILE_SIZE; x++) {
          putchar(tile != NULL ? tile->data[y * TILE_SIZE + x] : 'X');
        }
      }

      putchar('\n');
    }
  }
}

void align_tiles(struct tile* tiles, int count, struct tile*** out, int* out_size) {
  if (tiles == NULL || count == 0) return;

  int image_size = (int)sqrt((double)count);
  struct tile** image = (struct tile**)calloc(image_size * image_size, sizeof(struct tile*));
  if (image == NULL) return;

  image[0] = &tiles[0];
  tiles[0].y = 0;
  tiles[0].x = 0;

align:
  for (int i = 0; i < count; i++) {
    struct tile* first = &tiles[i];
    if (first->y < 0) continue;

    for (int j = 0; j < count; j++) {
      struct tile* second = &tiles[j];
      if (second->x >= 0) continue;
      if (is_known(image, image_size, first, ORIENTATION_TOP) && is_known(image, image_size, first, ORIENTATION_RIGHT) && is_known(image, image_size, first, ORIENTATION_BOTTOM) && is_known(image, image_size, first, ORIENTATION_LEFT)) continue;

      if (!is_known(image, image_size, first, ORIENTATION_RIGHT) && are_adjacent(first, second, ORIENTATION_RIGHT)) {
        second->x = first->x + 1;
        second->y = first->y;
        image[second->y * image_size + second->x] = second;
        continue;
      }
      else if (!is_known(image, image_size, first, ORIENTATION_BOTTOM) && are_adjacent(first, second, ORIENTATION_BOTTOM)) {
        second->x = first->x;
        second->y = first->y + 1;
        image[second->y * image_size + second->x] = second;
        continue;
      }
      else if (!is_known(image, image_size, first, ORIENTATION_TOP) && are_adjacent(first, second, ORIENTATION_TOP)) {
        if (first->y == 0) move_image(image, image_size, 0, 1);
        second->x = first->x;
        second->y = first->y - 1;
        image[second->y * image_size + second->x] = second;
        continue;
      }
      else if (!is_known(image, image_size, first, ORIENTATION_LEFT) && are_adjacent(first, second, ORIENTATION_LEFT)) {
        if (first->x == 0) move_image(image, image_size, 1, 0);
        second->x = first->x - 1;
        second->y = first->y;
        image[second->y * image_size + second->x] = second;
        continue;
      }
    }
  }

  for (int i = 0; i < image_size * image_size; i++) {
    if (image[i] == NULL) {
      goto align;
    }
  }

  *out = image;
  *out_size = image_size;
}

#pragma warning(disable: 6386)
char* get_image_without_borders(struct tile** image, int image_size, int* out_size) {
  int new_size = (TILE_SIZE - 2) * image_size;
  char* new_image = (char*)malloc(new_size * new_size);
  if (new_image == NULL) return NULL;

  int iy = 0;
  int ix = 0;
  
  for (int ir = 0; ir < image_size; ir++) {
    for (int y = 1; y < TILE_SIZE - 1; y++) {
      for (int t = 0; t < image_size; t++) {
        struct tile* tile = image[ir * image_size + t];

        for (int x = 1; x < TILE_SIZE - 1; x++) {
          new_image[iy * new_size + ix++] = tile->data[y * TILE_SIZE + x];
        }
      }
    }
  }

  *out_size = new_size;

  return new_image;
}
#pragma warning(default: 6386)

int count_monsters(char* image, int size) {
  int count = 0;

  for (int y = 0; y < size - MONSTER_HEIGHT; y++) {
    for (int x = 0; x < size - MONSTER_WIDTH; x++) {
      char* ii = &image[y * size + x];
      char* mi = MONSTER;

      int yi = y;
      int xi = x;

      while (*mi == ' ' || *ii == *mi) {
        ii++;
        mi++;

        if (*mi == '\0') count++;

        if (*mi == '\n') {
          yi++;
          xi = x;
          ii = &image[yi * size + xi];
          mi++;
        }
      }
    }
  }

  return count;
}

int get_monsters_in_image(char* image, int size) {
  int count = 0;

  for (int rotation = 0; rotation < 3; rotation++) {
    for (int orientation = 0; orientation < 2; orientation++) {
      count = count_monsters(image, size);

      if (count > 0) return count;

      reverse_tile(image, size, REVERSE_Y);
    }

    rotate_tile(image, size);
  }

  return count;
}

int main(int argc, char** argv) {
  struct tile* tiles = (struct tile*)malloc(200 * sizeof(struct tile));
  if (tiles == NULL) return 1;

  int count = 0;
  parse_input(tiles, &count);

  int image_size = 0;
  struct tile** image = NULL;
  align_tiles(tiles, count, &image, &image_size);

  printf("IDs of the four corner tiles multiplied together (Part 1): %llu\n", (image[0]->id) * (image[image_size - 1]->id) * (image[(image_size - 1) * image_size]->id) * (image[(image_size * image_size) - 1]->id));

  int new_size = 0;
  char* image_without_borders = get_image_without_borders(image, image_size, &new_size);
  int monster_count = get_monsters_in_image(image_without_borders, new_size);

  int hashtags_in_monster = 0;
  for (char* monster = MONSTER; *monster != '\0'; monster++) {
    if (*monster == '#') {
      hashtags_in_monster++;
    }
  }

  int hashtags_in_image = 0;
  for (int i = 0; i < new_size * new_size; i++) {
    if (image_without_borders[i] == '#') {
      hashtags_in_image++;
    }
  }

  printf("# that are not part of a sea monster (Part 2): %d\n", hashtags_in_image - (hashtags_in_monster * monster_count));

  free(tiles);
  free(image);
  free(image_without_borders);

  return 0;
}