function readInput(fileName)
  local file = io.open(fileName)
  local contents = { }

  while true do
    local line = file:read("*line")

    if not line then
      break
    end

    table.insert(contents, tonumber(line))
  end

  return contents
end

function getPartOne(fuelNeeded)
  local sum = 0

  for _, v in pairs(fuelNeeded) do
    sum = sum + math.floor(v / 3) - 2
  end

  return sum
end

function getPartTwo(fuelNeeded)
  local sum = 0

  while #fuelNeeded > 0 do
    local fuel = table.remove(fuelNeeded, 1)
    local mass = math.floor(fuel / 3) - 2

    if mass > 0 then
      sum = sum + mass
      table.insert(fuelNeeded, mass)
    end
  end

  return sum
end

function main()
  fuelNeeded = readInput("input.txt")

  print("Sum of the fuel requirements (Part 1): " .. getPartOne(fuelNeeded))
  print("Sum of the fuel requirements including fuel (Part 2): " .. getPartTwo(fuelNeeded))
end

main()