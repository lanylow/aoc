import java.io.*;
import java.util.*;

public class Main {
  private static class Range {
    private final int min, max;

    private Range(int _min, int _max) {
      this.min = _min;
      this.max = _max;
    }
  }

  private static List<List<Integer>> nearbyTickets;
  private static List<Integer> myTicket;

  private static Map<String, List<Range>> parse() throws IOException {
    Map<String, List<Range>> fields = new HashMap<>();
    var bufferedReader = new BufferedReader(new FileReader("input.txt"));

    String line = bufferedReader.readLine();

    while (!line.isEmpty()) {
      var colonIdx = line.indexOf(':');
      var dashIdx1 = line.indexOf('-');
      var dashIdx2 = line.lastIndexOf('-');
      var orIdx = line.lastIndexOf("or");

      var fieldName = line.substring(0, colonIdx);

      var first = new Range(Integer.parseInt(line.substring(colonIdx + 2, dashIdx1)), Integer.parseInt(line.substring(dashIdx1 + 1, orIdx - 1)));
      var second = new Range(Integer.parseInt(line.substring(orIdx + 3, dashIdx2)), Integer.parseInt(line.substring(dashIdx2 + 1)));

      fields.put(fieldName, new ArrayList<>(Arrays.asList(first, second)));

      line = bufferedReader.readLine();
    }

    bufferedReader.readLine();

    myTicket = new ArrayList<>();
    var ticket = bufferedReader.readLine();

    for (var n : ticket.split(",")) myTicket.add(Integer.parseInt(n));
    for (var i = 0; i < 2; i++) bufferedReader.readLine();

    nearbyTickets = new ArrayList<>();

    var nearby = bufferedReader.readLine();

    while (nearby != null) {
      var ns = nearby.split(",");
      List<Integer> rows = new ArrayList<>();

      for (var n : ns) rows.add(Integer.parseInt(n));

      nearbyTickets.add(rows);
      nearby = bufferedReader.readLine();
    }

    bufferedReader.close();

    return fields;
  }

  private static boolean is_in_range(Range r, int v) {
    return v >= r.min && v <= r.max;
  }

  private static int part1(Map<String, List<Range>> fields) {
    var sum = 0;

    for (var t : nearbyTickets) {
      outer_loop:

      for (var i = 0; i < t.size(); i++) {
        var query = t.get(i);

        for (var ranges : fields.values())
          for (var range : ranges)
            if (is_in_range(range, query))
              continue outer_loop;

        sum += query;
        t.set(i, -1);
      }
    }

    return sum;
  }

  private static List<String> findMatchingFields(Map<String, List<Range>> fields, List<Integer> column) {
    List<String> res = new ArrayList<>();

    for (var key : fields.keySet()) {
      var ranges = fields.get(key);

      var first = ranges.get(0);
      var second = ranges.get(1);

      var valid = true;

      for (var n : column) {
        if (!(is_in_range(first, n) || is_in_range(second, n))) {
          valid = false;
          break;
        }
      }

      if (valid) res.add(key);
    }

    return res;
  }

  private static List<List<String>> findValidFields(Map<String, List<Range>> fields, List<Integer> tickets) {
    List<List<String>> res = new ArrayList<>();

    for (var i = 0; i < tickets.size(); i++) {
      List<Integer> column = new ArrayList<>();

      for (var ticket : nearbyTickets) {
        var n = ticket.get(i);
        if (n != -1) column.add(n);
      }

      res.add(findMatchingFields(fields, column));
    }

    return res;
  }

  private static long part2(Map<String, List<Range>> fields, List<Integer> tickets) {
    List<String> identified = new ArrayList<>();
    List<List<String>> valid = findValidFields(fields, tickets);

    while (identified.size() != valid.size()) {
      for (var i = 0; i < valid.size(); i++) {
        var validFields = valid.get(i);

        if (validFields.size() == 1) {
          if (!identified.contains(validFields.get(0)))
            identified.add(validFields.get(0));
        } else {
          List<String> newFields = new ArrayList<>();

          for (var field : validFields)
            if (!identified.contains(field))
              newFields.add(field);

          valid.set(i, newFields);
        }
      }
    }

    var res = 1L;

    for (var i = 0; i < valid.size(); i++)
      if (valid.get(i).get(0).startsWith("departure"))
        res *= tickets.get(i);

    return res;
  }

  public static void main(String[] args) {
    try {
      Map<String, List<Range>> fields = parse();

      System.out.println("Ticket scanning error rate (Part 1): " + part1(fields));
      System.out.println("Six values multiplied together (Part 2): " + part2(fields, myTicket));
    } catch (IOException e) {
      System.out.println(e.getMessage());
    }
  }
}