fn get_input() -> Vec<i64> {
  return std::fs::read_to_string("input.txt").unwrap().split("\r\n\r\n").into_iter().map(|v| {
    v.lines().map(|l| l.parse::<i64>().unwrap()).sum() 
  }).collect();
}

fn get_most_calories(n: usize) -> i64 {
  let mut input = get_input();
  input.sort_by(|lhs, rhs| rhs.cmp(lhs));
  return input.into_iter().take(n).sum();
}

fn main() {
  println!("Part one: {}", get_most_calories(1));
  println!("Part two: {}", get_most_calories(3));
}