const input = require("fs").readFileSync("input.txt").toString("utf-8").split("").map(x => Number(x));

class Cup {
  constructor(label, next) {
    this.label = label;

    if (next) {
      this.back = next.back;
      this.front = next;
      
      this.back.front = this;
      this.front.back = this;
    } else {
      this.front = this;
      this.back = this;
    }
  }
}

class Circle {
  constructor(cups) {
    this.current = null;
    this.length = 0;
    this.labels = { };

    cups.forEach(cup => this.addCup(cup));
  }

  addCup(label) {
    const cup = new Cup(label, this.current);
  
    if (!this.current) {
      this.current = cup;
    }
  
    this.labels[label] = cup;
    this.length++;
  }
}

function getCupsAfter(cup, amount) {
  let cups = [];
  let front = cup.front;

  for (let i = 0; i < amount; i++) {
    cups = [...cups, front];
    front = front.front;
  }
  
  cup.front = front;
  front.back = cup;

  return cups;
}

function insertCupsAfter(cup, cups) {
  cups[0].back = cup;
  cups[cups.length - 1].front = cup.front;
  cup.front.back = cups[cups.length - 1];
  cup.front = cups[0];
}

function getDestinationCup(circle, current, cups) {
  const labels = cups.map(cup => cup.label);
  let destination = current.label;
  
  do {
    destination = (((destination - 2) + circle.length) % circle.length) + 1;
  } while(labels.includes(destination));
  
  return circle.labels[destination];
}

function simulateCrabMove(circle, current) {
  const cups = getCupsAfter(current, 3);
  const destination = getDestinationCup(circle, current, cups);
  insertCupsAfter(destination, cups);
}

function simulateGame(circle, rounds) {
  let current = circle.current;

  for (let i = 0; i < rounds; i++) {
    simulateCrabMove(circle, current);
    current = current.front;
  }
}

function circleToArray(circle) {
  if (!circle.current) {
    return [];
  }

  let labels = [circle.current.label];

  for (let cup = circle.current.front; cup !== circle.current; cup = cup.front) {
    labels = [...labels, cup.label];
  }

  return labels;
}

function getPartOne(cups) {
  const circle = new Circle(cups);
  simulateGame(circle, 100);
  circle.current = circle.labels[1];
  return Number(circleToArray(circle).slice(1).join(""));
}

function getPartTwo(cups) {
  const circle = new Circle([...cups, ...[...Array(1000001).keys()].slice(cups.length + 1)]);
  simulateGame(circle, 10000000);
  const cup = circle.labels[1];
  return cup.front.label * cup.front.front.label;
}

console.log("Labels on the cups after cup 1 (Part 1): ", getPartOne(input));
console.log("Labels multiplied together (Part 2): ", getPartTwo(input));