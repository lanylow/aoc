valid_passwords = 0
for line in open('day2.txt').readlines():
    data = line.split(' ')
    positions = data[0].split('-')
    required_char = data[1].strip(':')
    if data[2][int(positions[0]) - 1] == required_char and data[2][int(positions[1]) - 1] != required_char:
        valid_passwords += 1
    elif data[2][int(positions[0]) - 1] != required_char and data[2][int(positions[1]) - 1] == required_char:
        valid_passwords += 1
print(valid_passwords)
