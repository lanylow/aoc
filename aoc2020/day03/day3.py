file = open('day3.txt')
lines = [x.strip('\n') for x in file.readlines()]
file.close()

def count_trees(dy, dx):
    width = len(lines[0])
    height = len(lines)

    y = dy
    x = dx % width
    t = 0

    while y < height:
        if lines[y][x] == '#':
            t += 1
        y += dy
        x = (x + dx) % width

    return t

def count_slopes(m):
    r = 0
    for slope in m:
        t = count_trees(slope[0], slope[1])
        r = t if r == 0 else r * t
    return r

print("Part one: %d" % count_trees(1, 3))

slopes = [
    [ 1, 1 ],
    [ 1, 3 ],
    [ 1, 5 ],
    [ 1, 7 ],
    [ 2, 1 ]
]

print("Part two: %d" % count_slopes(slopes))
