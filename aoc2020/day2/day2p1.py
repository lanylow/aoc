valid_passwords = 0
for line in open('day2.txt').readlines():
    data = line.split(' ')
    required_amount = data[0].split('-')
    required_char = data[1].strip(':')
    containing_char = 0
    for char in data[2]:
        if char == required_char:
            containing_char += 1
    if containing_char >= int(required_amount[0]) and containing_char <= int(required_amount[1]):
        valid_passwords += 1
print(valid_passwords)
