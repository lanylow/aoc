fn parse() -> (isize, Vec<(isize, isize)>) {
  let input = std::fs::read_to_string("input.txt").unwrap();
  let mut lines = input.lines();

  return (lines.next().unwrap().parse().unwrap(), lines.next().unwrap().split(',').enumerate().filter_map(|(time, id)| {
    if id == "x" { None }
    else { Some((time as isize, id.parse().unwrap())) }
  }).collect());
}

fn part1(timestamp: &isize, ids: &Vec<(isize, isize)>) -> isize {
  let mut t = 0;

  loop {
    match ids.iter().find(|(_, id)| (t + timestamp) % id == 0).map(|(_, id)| id * t) {
      Some(time) => return time,
      None => t += 1
    }
  }
}

fn part2(ids: &Vec<(isize, isize)>) -> isize {
  let product: isize = ids.iter().map(|(_, id)| id).product();
  return ids.iter().map(|&(l, r)| -l * (product / r) * (0..r - 2).fold(1, |o, _| (o * (product / r)) % r)).sum::<isize>().rem_euclid(product);
}

fn main() {
  let (timestamp, ids) = parse();
  
  println!("ID of the earliest bus you can take to the airport multiplied by the number of minutes you'll need to wait for that bus (Part 1): {}", part1(&timestamp, &ids));
  println!("Earliest timestamp such that all of the listed bus IDs depart at offsets matching their positions in the list (Part 2): {}", part2(&ids));
}