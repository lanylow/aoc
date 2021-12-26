enum Action {
  North(isize),
  South(isize),
  East(isize),
  West(isize),
  Left(isize),
  Right(isize),
  Forward(isize)
}

#[derive(Copy, Clone)]
enum Direction {
  North,
  West,
  East,
  South
}

impl Direction {
  fn rotl(self, angle: isize) -> Direction {
    let mut res = self;
    let mut angle = angle;

    while angle >= 90 {
      res = match res {
        Direction::North => Direction::West,
        Direction::West => Direction::South,
        Direction::East => Direction::North,
        Direction::South => Direction::East
      };

      angle -= 90;
    }

    return res;
  }

  fn rotr(self, angle: isize) -> Direction {
    let mut res = self;
    let mut angle = angle;

    while angle >= 90 {
      res = match res {
        Direction::North => Direction::East,
        Direction::West => Direction::North,
        Direction::East => Direction::South,
        Direction::South => Direction::West
      };

      angle -= 90;
    }

    return res;
  }
}

fn parse() -> Vec<Action> {
  return std::fs::read_to_string("input.txt").unwrap().lines().map(|line| {
    let (action, number) = line.split_at(1);
    let value = number.parse().unwrap();

    match action {
      "N" => Action::North(value),
      "S" => Action::South(value),
      "E" => Action::East(value),
      "W" => Action::West(value),
      "L" => Action::Left(value),
      "R" => Action::Right(value),
      "F" => Action::Forward(value),
      _d => unreachable!()
    }
  }).collect();
}

fn waypoint_rotl(point: (isize, isize), angle: isize) -> (isize, isize) {
  let mut point = point;
  let mut angle = angle;

  while angle >= 90 {
    point = (-point.1, point.0);
    angle -= 90;
  }

  return point;
}

fn waypoint_rotr(point: (isize, isize), angle: isize) -> (isize, isize) {
  let mut point = point;
  let mut angle = angle;

  while angle >= 90 {
    point = (point.1, -point.0);
    angle -= 90;
  }

  return point;
}

fn part1(actions: &[Action]) -> isize {
  let mut dir = Direction::East;
  let mut pos = (0isize, 0isize);

  for action in actions.iter() {
    let (ndir, npos) = match action {
      Action::North(v) => (dir, (pos.0, pos.1 + *v)),
      Action::South(v) => (dir, (pos.0, pos.1 - *v)),
      Action::East(v) => (dir, (pos.0 + *v, pos.1)),
      Action::West(v) => (dir, (pos.0 - *v, pos.1)),

      Action::Left(v) => (dir.rotl(*v), pos),
      Action::Right(v) => (dir.rotr(*v), pos),
      Action::Forward(v) =>  ( dir, match dir {
        Direction::North => (pos.0, pos.1 + *v),
        Direction::South => (pos.0, pos.1 - *v),
        Direction::East => (pos.0 + *v, pos.1),
        Direction::West => (pos.0 - *v, pos.1)
      })
    };

    dir = ndir;
    pos = npos;
  }

  return pos.0.abs() + pos.1.abs();
}

fn part2(actions: &[Action]) -> isize {
  let mut point = (10isize, 1isize);
  let mut pos = (0isize, 0isize);

  for action in actions.iter() {
    let (npos, npoint) = match action {
      Action::North(v) => (pos, (point.0, point.1 + *v)),
      Action::South(v) => (pos, (point.0, point.1 - *v)),
      Action::East(v) => (pos, (point.0 + *v, point.1)),
      Action::West(v) => (pos, (point.0 - *v, point.1)),

      Action::Left(v) => (pos, waypoint_rotl(point, *v)),
      Action::Right(v) => (pos, waypoint_rotr(point, *v)),
      Action::Forward(v) =>  ((pos.0 + point.0 * *v, pos.1 + point.1 * *v), point)
    };

    pos = npos;
    point = npoint;
  }

  return pos.0.abs() + pos.1.abs();
}

fn main() {
  let actions = parse();

  println!("Manhattan distance between current location and the ship's starting position (Part 1): {}", part1(&actions));
  println!("Manhattan distance between waypoint location and the ship's starting position (Part 2): {}", part2(&actions));
}