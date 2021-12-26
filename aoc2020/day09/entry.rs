fn parse() -> Vec<usize> {
  let input = std::fs::read_to_string("input.txt").unwrap();
  input.lines().map(|line| line.parse().unwrap()).collect()
}

fn part1(numbers: &[usize], preamble: usize) -> usize {
  for i in preamble..numbers.len() {
    let n = numbers[i];
    let mut found = false;

    for j in i - preamble..i {
      for k in j..i {
        if numbers[j] + numbers[k] == n {
          found = true;
          break;
        }
      }

      if found { break; }
    }

    if !found { return n; }
  }

  unreachable!();
}

fn part2(numbers: &[usize], invalid: usize) -> usize {
  for i in 0..numbers.len() {
    let mut sum = numbers[i];
    let mut end = i + 1;

    let res = loop {
      sum += numbers[end];

      if sum == invalid { break Some(end); }
      if sum > invalid { break None; }

      end += 1;
    };

    if let Some(res) = res {
      return numbers[i..=res].iter().min().unwrap() + numbers[i..=res].iter().max().unwrap();
    }
  }

  unreachable!();
}

fn main() {
  let numbers = parse();
  let invalid = part1(&numbers, 25);

  println!("First number that does not have this property (Part 1): {}", invalid);
  println!("Encryption weakness in your XMAS-encrypted list of numbers (Part 2): {}", part2(&numbers, invalid));
}