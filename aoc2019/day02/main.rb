def simulateProgram(instructions)
  programCounter = 0

  while instructions[programCounter] != 99 do
    instruction = instructions[programCounter]

    if instruction == 1
      instructions[instructions[programCounter + 3]] = instructions[instructions[programCounter + 1]] + instructions[instructions[programCounter + 2]]
    elsif instruction == 2
      instructions[instructions[programCounter + 3]] = instructions[instructions[programCounter + 1]] * instructions[instructions[programCounter + 2]]
    end

    programCounter += 4
  end

  return instructions[0]
end

def getPartOne(instructions)
  instructions[1] = 12
  instructions[2] = 2

  return simulateProgram(instructions)
end

def getPartTwo(instructions)
  for noun in 0..99 do
    for verb in 0..99 do
      instructions[1] = noun
      instructions[2] = verb

      if simulateProgram(instructions.dup) == 19690720
        return 100 * noun + verb
      end
    end
  end
end

def main()
  instructions = File.read("input.txt").split(",").map(&:to_i)

  puts("Value left at position 0 (Part 1): #{getPartOne(instructions.dup)}")
  puts("100 * noun + verb (Part 2): #{getPartTwo(instructions.dup)}")
end

main()