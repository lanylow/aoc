fn parse() -> Vec<usize> {
  let input = std::fs::read_to_string("input.txt").unwrap();
  return input.trim().split(',').map(|number| number.parse().unwrap()).collect();
}

fn run(max: usize, numbers: &[usize]) -> usize {
  let mut last_spoken: Vec<Option<usize>> = vec![None; max];
  let mut last_number = None;

  for i in 0..max {
    let n = if i < numbers.len() {
      numbers[i as usize]
    } 
    else {
      let last_number = last_number.unwrap();
      last_spoken[last_number as usize].map(|number| i - number - 1).unwrap_or(0)
    };

    if let Some(last_number) = last_number { last_spoken[last_number as usize] = Some(i - 1); }
    last_number = Some(n);
  }

  return last_number.unwrap();
}

fn main() {
  let numbers = parse();

  println!("2020th number spoken (Part 1): {}", run(2020, &numbers));
  println!("30000000th number spoken (Part 2): {}", run(30000000, &numbers));
}