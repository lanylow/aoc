#[derive(Clone, Copy, Eq, PartialEq)]
enum PosType {
  Floor,
  Empty,
  Occupied,
  Oob
}

#[derive(Clone, Eq, PartialEq)]
struct Pos {
  v: Vec<PosType>,
  w: usize,
  h: usize
}

impl Pos {
  fn get_adjacent(&self, x: usize, y: usize) -> impl Iterator<Item = PosType> + '_ {
    return (-1..=1).flat_map(move |lx| (-1..=1).map(move |ly| (x as isize + lx, y as isize + ly))).filter(move |(ux, uy)| *ux != x as isize || *uy != y as isize).map(move |(x, y)| {
      if x >= self.w as isize || x < 0 || y >= self.h as isize || y < 0 { PosType::Oob }
      else { self.v[y as usize * self.w + x as usize] }
    });
  }

  fn get_closest_seat(&self, x: usize, y: usize, dirx: isize, diry: isize) -> Option<(usize, usize)> {
    let mut x = x as isize + dirx;
    let mut y = y as isize + diry;

    while x >= 0 && y >= 0 && x < self.w as isize && y < self.h as isize {
      match self.v[y as usize * self.w + x as usize] {
        PosType::Occupied | PosType::Empty => return Some((x as usize, y as usize)),
        _d => { }
      }

      x += dirx;
      y += diry;
    }

    return None;
  }

  fn get_sight(&self) -> Vec<Vec<usize>> {
    return (0..self.h).into_iter().flat_map(|y| (0..self.w).into_iter().map(move |x| (x, y))).map(|(x, y)| {
      (-1..=1).into_iter().flat_map(|dirx| (-1..=1).into_iter().map(move |diry| (dirx, diry))).filter(move |(dirx, diry)| *dirx != 0 || *diry != 0).filter_map(|(dirx, diry)| self.get_closest_seat(x, y, dirx, diry)).map(|(x, y)| y * self.w + x).collect()
    }).collect()
  }
}

fn parse() -> Pos {
  let input = std::fs::read_to_string("input.txt").unwrap();

  let pos: Vec<Vec<PosType>> = input.lines().map(|line| {
    line.chars().map(|c| match c {
      '.' => PosType::Floor,
      'L' => PosType::Empty,
      '#' => PosType::Occupied,
      d => unreachable!("{}", d)
    }).collect()
  }).collect();

  let w = pos[0].len();
  let h = pos.len();

  Pos {
    v: pos.into_iter().flat_map(|v| v.into_iter()).collect(),
    w,
    h
  }
}

fn process(pos: &Pos) -> Pos {
  let mut new = Pos {
    v: vec![PosType::Floor; pos.v.len()],
    w: pos.w,
    h: pos.h
  };

  for i in 0..new.h {
    for j in 0..new.w {
      let idx = i * pos.w + j;

      new.v[idx] = match pos.v[idx] {
        PosType::Empty => {
          if pos.get_adjacent(j, i).any(|p| p == PosType::Occupied) { PosType::Empty }
          else { PosType::Occupied }
        }

        PosType::Occupied => {
          if pos.get_adjacent(j, i).filter(|p| *p == PosType::Occupied).count() >= 4 { PosType::Empty }
          else { PosType::Occupied }
        }

        d => d
      }
    }
  }

  return new;
}

fn process_with_sight(pos: &Pos, sight: &[Vec<usize>]) -> Pos {
  let mut new = Pos {
    v: vec![PosType::Floor; pos.v.len()],
    w: pos.w,
    h: pos.h
  };

  for i in 0..new.h {
    for j in 0..new.w {
      let idx = i * pos.w + j;

      new.v[idx] = match pos.v[idx] {
        PosType::Empty => {
          if sight[idx].iter().map(|idx| pos.v[*idx]).any(|p| p == PosType::Occupied) { PosType::Empty }
          else { PosType::Occupied }
        }

        PosType::Occupied => {
          if sight[idx].iter().map(|idx| pos.v[*idx]).filter(|p| *p == PosType::Occupied).count() >= 5 { PosType::Empty }
          else { PosType::Occupied }
        }

        d => d
      }
    }
  }

  return new;
}

fn part1(pos: &Pos) -> usize {
  return {
    let mut pos = pos.clone();

    loop {
      let new = process(&pos);
      if new == pos { break new.v.into_iter().filter(|p| *p == PosType::Occupied).count(); }
      pos = new;
    }
  };
}

fn part2(pos: Pos) -> usize {
  let sight = pos.get_sight();

  return {
    let mut pos = pos;

    loop {
      let new = process_with_sight(&pos, &sight);
      if new == pos { break new.v.into_iter().filter(|p| *p == PosType::Occupied).count(); }
      pos = new;
    }
  };
}

fn main() {
  let pos = parse();

  println!("Amount of seats occupied (Part 1): {}", part1(&pos));
  println!("Amount of seats occupied (Part 2): {}", part2(pos));
}