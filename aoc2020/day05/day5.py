ls = [x.strip('\n') for x in open('day5.txt').readlines()]

def find_num(t, n, l, lc, uc, o):
    rl = 0
    rh = n
    for i in range(l):
        if t[i + o] == lc:
            rh = ((rh + rl) / 2) - 0.5
        elif t[i + o] == uc:
            rl = rl + ((rh - rl) / 2) + 0.5
    return rl

def get_seat_ids():
    s = [ ]
    for l in ls:
        r = find_num(l, 127, 7, 'F', 'B', 0)
        c = find_num(l, 7, 3, 'L', 'R', 7)
        sid = int(r * 8 + c)
        s.append(sid)
    return s

def part_one():
    h = 0
    for s in get_seat_ids():
        if s > h:
            h = s
    return h

def part_two():
    s = get_seat_ids()
    s.sort()
    for i in range(len(s) - 1):
        if s[i] + 1 != s[i + 1]:
            return s[i] + 1

print("Part one: %d" % part_one())
print("Part two: %d" % part_two())
