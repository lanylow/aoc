def parse_file(file_name):
    ls = [x.strip('\n') for x in open(file_name).readlines()]
    r = [ ]
    c = ''
    for l in ls:
        if l:
            c += ' ' + l
        elif not l:
            r.append(c)
            c = ''
    return r

def part_one(f):
    r = 0
    for l in f:
        u = [ ]
        for c in l:
            if c != ' ' and c not in u:
                u.append(c)
        r += len(u)
    return r

def part_two(f):
    r = 0
    for l in f:
        s = l.strip().split(' ')
        n = len(s)
        a = { }
        for p in s:
            for c in p:
                if c in a:
                    a[c] += 1
                else:
                    a[c] = 1
        for v in a.values():
            if n == v:
                r += 1
    return r

f = parse_file('day6.txt')

print("Part one: %d" % part_one(f))
print("Part two: %d" % part_two(f))
