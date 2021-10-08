class inst:
    def __init__(self, op, arg):
        self.op = op
        self.arg = arg

    def get_arg(self):
        v = int(self.arg[1:])
        if self.arg[0] == '-':
            return -v
        return v

def parse_program(fn):
    ls = [x.strip('\n') for x in open(fn).readlines()]
    r = [ ] 
    for l in ls:
        mnemonic = l.split(' ')
        r.append(inst(mnemonic[0], mnemonic[1]))
    return r

def emulate_program(p):
    ac = 0
    pc = 0
    ex = [ ]
    while pc < len(p) and pc not in ex:
        ex.append(pc)
        if p[pc].op == 'nop':
            pc += 1
        elif p[pc].op == 'acc':
            ac += p[pc].get_arg()
            pc += 1
        elif p[pc].op == 'jmp':
            pc += p[pc].get_arg()
    return ac, ex, pc >= len(p)

def part_one(p):
    return emulate_program(p)[0]

def reverse_inst(op):
    if op == 'jmp':
        return 'nop'
    return 'jmp'

def part_two(p):
    ex = emulate_program(p)[1]
    for i in ex:
        if p[i].op != 'acc':
            p[i].op = reverse_inst(p[i].op)
            r = emulate_program(p)
            if r[2]:
                return r[0]
            p[i].op = reverse_inst(p[i].op)

p = parse_program('day8.txt')

print("Part one: %d" % part_one(p))
print("Part two: %d" % part_two(p))
