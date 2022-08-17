import std.stdio;
import std.conv;
import std.range;
import std.algorithm;

bool meetsCriteria(int number) {
  auto digits = number.to!string.split("").map!(to!int);
  int previousDigit = digits.front;

  foreach (i; digits.dropOne) {
    if (previousDigit > i) {
      return false;
    }

    previousDigit = i;
  }

  previousDigit = digits.front;

  foreach (i; digits.dropOne) {
    if (previousDigit == i) {
      return true;
    }

    previousDigit = i;
  }

  return false;
}

bool digitsRepeatTwoTimes(int number) {
  auto digits = number.to!string.split("").map!(to!int);

  foreach (i; digits.dropOne) {
    if (digits.count(i) == 2) {
      return true;
    }
  }

  return false;
}

int getPartOne(int minimum, int maximum) {
  int result = 0;
  
  foreach (i; minimum .. maximum) {
    if (meetsCriteria(i)) {
      result++;
    }
  }

  return result;
}

int getPartTwo(int minimum, int maximum) {
  int result = 0;
  
  foreach (i; minimum .. maximum) {
    if (meetsCriteria(i) && digitsRepeatTwoTimes(i)) {
      result++;
    }
  }

  return result;
}

void main() {
  int minimum = 146_810;
  int maximum = 612_564;

  writefln("Passwords that meet the criteria (Part 1): %d", getPartOne(minimum, maximum));
  writefln("Passwords that meet all the criteria (Part 1): %d", getPartTwo(minimum, maximum));
}