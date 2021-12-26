import re

def parse_file(file_name):
    f = open(file_name)
    ls = [x.strip('\n') for x in f.readlines()]
    f.close()

    r = [ ]
    c = ""

    for l in ls:
        if l:
            c += " " + l
        elif not l:
            r.append(c)
            c = ""

    return r

def is_between(v, l, h):
    return l <= v and v <= h

def part_1(passports):
    r = 0
    for p in passports:
        if all(x in p for x in [ 'byr', 'iyr', 'eyr', 'hgt', 'hcl', 'ecl', 'pid' ]):
            r += 1
    print(r)

def part_2(passports):
    r = 0
    for p in passports:
        d = p.split(' ')
        v = 0
        for f in d:
            split = f.split(':')
            if re.match('byr:\d{4}$', f):
                if is_between(int(split[1]), 1920, 2002):
                    v += 1
            elif re.match('iyr:\d{4}$', f):
                if is_between(int(split[1]), 2010, 2020):
                    v += 1
            elif re.match('eyr:\d{4}$', f):
                if is_between(int(split[1]), 2020, 2030):
                    v += 1
            elif re.match('hgt:\d{2,3}(cm|in)$', f):
                unit = split[1][-2:]
                split[1] = split[1].strip(unit)
                height = int(split[1])
                if unit == "cm":
                    if is_between(height, 150, 193):
                        v += 1
                elif unit == "in":
                    if is_between(height, 59, 76):
                        v += 1
            elif re.match('hcl:#[\d|\w]{6}$', f):
                v += 1
            elif re.match('ecl:(amb|blu|brn|gry|grn|hzl|oth)$', f):
                v += 1
            elif re.match('pid:\d{9}$', f):
                v += 1
        if v >= 7:
            r += 1
    print(r)

passports = parse_file('day4.txt')

part_1(passports)
part_2(passports)
