use std::collections::HashMap;

enum Rule {
  Data(Vec<usize>),
  Cond(Vec<Rule>),
  Char(char)
}

fn parse_single_rule(data: &str) -> Rule {
  let data = data.trim();

  if data.contains('|') {
    let split = data.split('|').peekable();
    return Rule::Cond(split.map(parse_single_rule).collect());
  } else if data.starts_with('"') {
    return Rule::Char(data.chars().nth(1).unwrap());
  } else {
    return Rule::Data(data.split_ascii_whitespace().map(|s| s.parse().unwrap()).collect());
  }
}

fn parse_rule(line: &str) -> (usize, Rule) {
  let mut split = line.split(':');
  let idx = split.next().unwrap().parse().unwrap();
  return (idx, parse_single_rule(split.next().unwrap()));
}

fn parse(input: &str) -> (HashMap<usize, Rule>, Vec<&str>) {
  let mut lines = input.lines();
  let mut rules = HashMap::new();

  while let Some(line) = lines.next() {
    if line.is_empty() { break; }
    let (idx, rule) = parse_rule(line);
    rules.insert(idx, rule);
  }

  return (rules, lines.collect());
}

fn check<'a>(chars: &'a [char], rule: &Rule, rules: &HashMap<usize, Rule>) -> Option<Vec<&'a [char]>> {
  match rule {
    Rule::Char(c) => {
      if *chars.first()? == *c { return Some(vec![&chars[1..]]); }
      else { return None; }
    }

    Rule::Cond(c) => {
      let mut res = c.iter().filter_map(|s| check(chars, s, rules)).peekable();
      if res.peek().is_some() { return Some(res.flatten().collect()); }
      else { return None; }
    }

    Rule::Data(d) => {
      let mut res = vec![chars];

      for e in d {
        let mut new_res = res.into_iter().filter_map(|p| check(&p, &rules[e], rules)).peekable();
        if new_res.peek().is_some() { res = new_res.flatten().collect(); }
        else { return None; }
      }

      return Some(res);
    }
  }
}

fn get_result(file_name: &str) -> usize {
  let input = std::fs::read_to_string(file_name).unwrap();
  let (rules, input) = parse(&input);

  return input.iter().filter(|s| {
    return check(&s.chars().collect::<Vec<_>>(), &rules[&0], &rules).map(|res| res.iter().any(|r| r.is_empty())).unwrap_or(false);
  }).count();
}

fn main() {
  println!("Amount of messages that completely match rule 0 (Part 1): {}", get_result("input.txt"));
  println!("Amount of messages that completely match rule 0 after updating rules 8 and 11 (Part 2): {}", get_result("input_replaced.txt"));
}