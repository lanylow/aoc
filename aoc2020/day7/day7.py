def read_file(fn):
    return [x.strip('\n').strip('.') for x in open(fn).readlines()]

def parse_lines(ls):
    r = { }
    for l in ls:
        t = l.split('contain')[0].replace(' bags ', '')
        r[t] = { }
        s = l.split('contain')[1].split(',')
        for b in s:
            bs = b.strip()
            if bs == 'no other bags':
                break
            bs = bs.replace(' bags', '').replace(' bag', '')
            n = bs[0]
            r[t][bs[2:]] = n
    return r

bs = parse_lines(read_file('day7.txt'))

def check_bags(bv):
    for k, v in bv.items():
        if k == 'shiny gold':
            return True
        if check_bags(bs[k]):
            return True

def part_one():
    r = 0
    for bk, bv in bs.items():
        if bk == 'shiny gold':
            continue
        if check_bags(bv):
            r += 1
    return r

def count_bags(bv):
    r = 0
    for k, v in bv.items():
        for i in range(int(v)):
            r += count_bags(bs[k])
        r += int(v)
    return r

def part_two():
    return count_bags(bs['shiny gold'])

print("Part one: %d" % part_one())
print("Part two: %d" % part_two())
