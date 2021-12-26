fn parse() -> Vec<usize> {
  let input = std::fs::read_to_string("input.txt").unwrap();
  return input.lines().map(|line| line.parse().unwrap()).collect();
}

fn part1(numbers: &[usize]) -> usize {
  let mut jumps = [0, 0, 1];
  let mut jolts = 0;

  for i in numbers.iter() {
    let jump = i - jolts;
    jumps[jump - 1] += 1;
    jolts = *i;
  }

  return jumps[0] * jumps[2];
}

fn part2(numbers: &[usize]) -> usize {
  let mut ways = vec![0usize; numbers.len()];
  ways[0] = 1;
  
  for i in 0..numbers.len() {
    let way = ways[i];
    let rate = numbers[i];

    for j in 1..=3 {
      if i + j >= numbers.len() || numbers[i + j] - rate > 3 { break; }
      ways[i + j] += way;
    }
  }

  return *ways.last().unwrap();
}

fn main() {
  let mut numbers = parse();
  numbers.sort_unstable();

  println!("Number of 1-jolt differences multiplied by the number of 3-jolt differences (Part 1): {}", part1(&numbers));

  numbers.insert(0, 0);
  println!("Total number of distinct ways you can arrange the adapters to connect the charging outlet to your device (Part 2): {}", part2(&numbers));
}