#include <stdio.h>
#include <stdint.h>
#include <stdlib.h>
#include <string.h>

struct vec3i {
  int x, y, z;
};

struct vec3i_set {
  struct vec3i vecs[1024];
  int cnt;
};

struct vec3i_set table[30];
struct vec3i_set cur = { 0 };
struct vec3i_set cur_dist[1024] = { 0 };
int scanners = 0;

int vec3i_sum(struct vec3i v) { return v.x + v.y + v.z; }

int vec3i_sqrlen(struct vec3i v) { return v.x * v.x + v.y * v.y + v.z * v.z; }

struct vec3i vec3i_sub(struct vec3i l, struct vec3i r) { return (struct vec3i) { l.x - r.x, l.y - r.y, l.z - r.z }; }

struct vec3i vec3i_add(struct vec3i l, struct vec3i r) { return (struct vec3i) { l.x + r.x, l.y + r.y, l.z + r.z }; }

struct vec3i vec3i_abs(struct vec3i v) { return (struct vec3i) { abs(v.x), abs(v.y), abs(v.z) }; }

struct vec3i vec3i_orient(struct vec3i v, int o) {
  int x = v.x, y = v.y, z = v.z;

  struct vec3i ot[24] = {
    { +x, +y, +z }, 
    { +y, +z, +x }, 
    { +z, +x, +y }, 
    { +z, +y, -x }, 
    { +y, +x, -z }, 
    { +x, +z, -y },
    { +x, -y, -z }, 
    { +y, -z, -x }, 
    { +z, -x, -y }, 
    { +z, -y, +x }, 
    { +y, -x, +z }, 
    { +x, -z, +y },
    { -x, +y, -z }, 
    { -y, +z, -x }, 
    { -z, +x, -y }, 
    { -z, +y, +x }, 
    { -y, +x, +z }, 
    { -x, +z, +y },
    { -x, -y, +z }, 
    { -y, -z, +x }, 
    { -z, -x, +y }, 
    { -z, -y, -x }, 
    { -y, -x, -z }, 
    { -x, -z, -y }
  };

  return ot[o];
}

int vec3i_set_exists(struct vec3i_set* vs, struct vec3i v) {
  for (int i = 0; i < vs->cnt; i++) {
    struct vec3i cv = vs->vecs[i];

    if (cv.x == v.x && cv.y == v.y && cv.z == v.z)
      return 1;
  }

  return 0;
}

void vec3i_set_add(struct vec3i_set* vs, struct vec3i v) {
  if (vec3i_set_exists(vs, v))
    return;

  vs->vecs[vs->cnt++] = v;
}

int vec3i_set_count_common(struct vec3i_set* vsl, struct vec3i_set* vsr) {
  int cnt = 0;

  for (int i = 0; i < vsl->cnt; i++)
    if (vec3i_set_exists(vsr, vsl->vecs[i]))
      cnt++;

  return cnt;
}

void vec3i_set_orient(struct vec3i_set* out, struct vec3i_set* in, int o) {
  *out = *in;

  for (int i = 0; i < out->cnt; i++)
    out->vecs[i] = vec3i_orient(out->vecs[i], o);
}

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
    while (fgetstripped(buf, 2048, file)) {
      if (buf[0] == '\0')
        break;

      int x, y, z;
      sscanf(buf, "%d,%d,%d", &x, &y, &z);
      vec3i_set_add(&table[scanners], (struct vec3i) { x, y, z });
    }

    scanners++;
  }

  fclose(file);
}

void init_distances(struct vec3i_set* out, struct vec3i_set* in, struct vec3i p) {
  for (int i = 0; i < in->cnt; i++) {
    struct vec3i off = { vec3i_sqrlen(vec3i_sub(in->vecs[i], p)), 0, 0 };
    out->vecs[i] = off;
  }

  out->cnt = in->cnt;
}

void init_offs(struct vec3i_set* out, struct vec3i_set* in, struct vec3i p) {
  for (int i = 0; i < in->cnt; i++)
    out->vecs[i] = vec3i_sub(in->vecs[i], p);

  out->cnt = in->cnt;
}

void update_distances() {
  for (int i = 0; i < cur.cnt; i++)
    init_distances(&cur_dist[i], &cur, cur.vecs[i]);
}

int enough_commons(struct vec3i_set* vs, struct vec3i* l, struct vec3i* r) {
  struct vec3i_set off;

  for (int i = 0; i < vs->cnt; i++) {
    struct vec3i p = vs->vecs[i];
    init_distances(&off, vs, p);

    for (int j = 0; j < cur.cnt; j++) {
      struct vec3i p1 = cur.vecs[j];

      if (vec3i_set_count_common(&off, &cur_dist[j]) >= 12) {
        *l = p1;
        *r = p;
        return 1;
      }
    }
  }

  return 0;
}

int match_scanners(struct vec3i_set* vs, struct vec3i* off, int* o) {
  struct vec3i l, r;

  if (!enough_commons(vs, &l, &r))
    return 0;

  struct vec3i_set off1, off2;

  init_offs(&off1, &cur, l);
  init_offs(&off2, vs, r);

  struct vec3i_set oriented = { 0 };

  for (int i = 0; i < 24; i++) {
    vec3i_set_orient(&oriented, &off2, i);

    if (vec3i_set_count_common(&off1, &oriented) >= 12) {
      *off = vec3i_sub(l, vec3i_orient(r, i));
      *o = i;
      return 1;
    }
  }

  return 0;
}

int solve() {
  struct vec3i_set locs = { 0 };
  int done_cnt = 1, o;
  int done[30] = { 1 };

  for (int i = 0; done_cnt < scanners; i = (i + 1) % scanners) {
    if (done[i])
      continue;

    struct vec3i off;
    struct vec3i_set curr = table[i];

    if (match_scanners(&curr, &off, &o)) {
      vec3i_set_add(&locs, off);

      for (int j = 0; j < curr.cnt; j++) {
        struct vec3i p = curr.vecs[j];
        vec3i_set_add(&cur, vec3i_add(vec3i_orient(p, o), off));
      }

      update_distances();
      done[i] = 1;
      done_cnt++;
    }
  }

  int res = 0;

  for (int i = 0; i < locs.cnt; i++) {
    for (int j = 0; j < locs.cnt; j++) {
      int dst = vec3i_sum(vec3i_abs(vec3i_sub(locs.vecs[i], locs.vecs[j])));
      res = max(res, dst);
    }
  }

  return res;
}

int main(int argc, char** argv) {
  parse();
  cur = table[0];
  update_distances();

  int max = solve();

  printf("Count of beacons (Part 1): %d\n", cur.cnt);
  printf("Largest Manhattan distance between any two scanners (Part 2): %d\n", max);

  system("pause");
}