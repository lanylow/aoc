import java.io.*;
import java.util.*;

public class Main {
  private static long getResult(String line) {
    Stack<String> stack = new Stack<>();

    for (var c : line.toCharArray()) {
      if (c == '+' || c == '*') {
        long lhs = Long.parseLong(stack.pop()), rhs = Long.parseLong(stack.pop());

        if (c == '+') stack.push(String.valueOf(lhs + rhs));
        else stack.push(String.valueOf(lhs * rhs));
      } else {
        stack.push(String.valueOf(c));
      }
    }

    return Long.parseLong(stack.pop());
  }

  private static int getOrder(char c) {
    return c == '+' ? 1 : -1;
  }

  private static String parseLine(String line, boolean plusFirst) {
    var stringBuilder = new StringBuilder();
    Stack<Character> stack = new Stack<>();

    for (var c : line.toCharArray()) {
      switch (c) {
        case '+':
        case '*': {
          if (stack.empty() || stack.peek() == '(') {
            stack.push(c);
          } else if ((!plusFirst && (stack.peek() == '+' || stack.peek() == '*')) || (plusFirst && (getOrder(stack.peek()) >= getOrder(c)))) {
            stringBuilder.append(stack.pop());
            stack.push(c);
          } else {
            stack.push(c);
          }

          break;
        }

        case '(': {
          stack.push(c);
          break;
        }

        case ')': {
          while (stack.peek() != '(') stringBuilder.append(stack.pop());
          stack.pop();
          break;
        }

        default: {
          if (c != ' ') stringBuilder.append(c);
          break;
        }
      }
    }

    while (!stack.empty()) stringBuilder.append(stack.pop());

    return stringBuilder.toString();
  }

  public static void main(String[] args) throws IOException {
    var bufferedReader = new BufferedReader(new FileReader("input.txt"));
    var line = bufferedReader.readLine();

    var part1 = 0L;
    var part2 = 0L;

    while (line != null) {
      part1 += getResult(parseLine(line, false));
      part2 += getResult(parseLine(line, true));

      line = bufferedReader.readLine();
    }

    bufferedReader.close();

    System.out.println("Sum of the resulting values (Part 1): " + part1);
    System.out.println("Results of evaluating the homework problems using these new rules (Part 2): " + part2);
  }
}
