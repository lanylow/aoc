def get_ints(file):
    with open(file) as f:
        return [int(x.strip('\n')) for x in f.readlines()]

def get_part_one(ints):
    for i in ints:
        for j in ints:
            if i + j == 2020:
                return i * j

def get_part_two(ints):
    for i in ints:
        for j in ints:
            for k in ints:
                if i + j + k == 2020:
                    return i * j * k

ints = get_ints('day1.txt')
print("Part one: %d" % get_part_one(ints))
print("Part two: %d" % get_part_two(ints))
