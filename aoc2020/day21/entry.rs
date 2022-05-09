use std::collections::{HashSet, HashMap};

struct Food<'a> {
  ingredients: HashSet<&'a str>,
  allergens: HashSet<&'a str>
}

fn parse(data: &str) -> Vec<Food> {
  return data.lines().map(|l| {
    let split = l.split("(contains ").collect::<Vec<_>>();
    let ingredients = split[0].trim().split(' ').collect();
    let mut allergens = HashSet::default();

    if split.len() > 1 {
      let len = split[1].len();
      allergens = split[1][..len-1].trim().split(", ").collect();
    }

    return Food {
      ingredients,
      allergens
    };
  }).collect();
}

fn find_common<'a>(food: &'a[Food]) -> HashMap<&'a str, HashSet<&'a str>> {
  let mut out: HashMap<&'a str, HashSet<&'a str>> = HashMap::default();

  for f in food {
    for allergen in &f.allergens {
      let common = match out.get(*allergen) {
        None => f.ingredients.clone(),
        Some(i) => i.intersection(&f.ingredients).copied().collect()
      };

      out.insert(*allergen, common);
    }
  }

  return out;
}

fn get_allergens<'a>(common: &'a HashMap<&'a str, HashSet<&'a str>>) -> HashMap<&'a str, &'a str> {
  let mut out: HashMap<&'a str, &'a str> = HashMap::default();
  let count = common.len();

  while out.len() != count {
    let left = common.keys().filter(|&k| !out.contains_key(k)).collect::<Vec<_>>();

    for &allergen in left {
      let res = out.values().cloned().collect();
      let possible = common[allergen].difference(&res).collect::<Vec<_>>();

      if possible.len() == 1 {
        out.insert(allergen, *possible[0]);
      }
    }
  }

  return out;
}

fn main() {
  let data = std::fs::read_to_string("input.txt").unwrap();
  let food = parse(&data);
  let common = find_common(&food);
  let allergens = get_allergens(&common);
  let excluded: HashSet<&str> = allergens.values().copied().collect();

  println!("Part 1: {}", food.iter().map(|f| f.ingredients.difference(&excluded).count()).sum::<usize>());

  let mut list = allergens.iter().collect::<Vec<_>>();
  list.sort_by_key(|k| k.0);

  println!("Part 2: {}", list.iter().map(|k| *k.1).collect::<Vec<_>>().join(","));
}