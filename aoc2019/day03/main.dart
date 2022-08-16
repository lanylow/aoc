import 'dart:io';
import 'dart:math';

Map<Point<int>, int> getCrossingPoints(List<String> wires) {
  var points = <Point<int>, int>{};
  var crosses = <Point<int>, int>{};

  for (var wire in wires) {
    var pointsDistance = <Point<int>, int>{};

    var wirePoint = Point(0, 0);
    var wireDistance = 0;

    for (var path in wire.split(',')) {
      for (var i = 0; i < int.parse(path.substring(1)); i++) {
        switch (path[0]) {
          case 'U':
            wirePoint = Point(wirePoint.x, wirePoint.y + 1);
            break;

          case 'D':
            wirePoint = Point(wirePoint.x, wirePoint.y - 1);
            break;

          case 'L':
            wirePoint = Point(wirePoint.x - 1, wirePoint.y);
            break;

          case 'R':
            wirePoint = Point(wirePoint.x + 1, wirePoint.y);
            break;
        }

        wireDistance += 1;

        if (!pointsDistance.containsKey(wirePoint)) {
          pointsDistance[wirePoint] = wireDistance;
        }
      }
    }

    for (var point in pointsDistance.entries) {
      if (!points.containsKey(point.key)) {
        points[point.key] = point.value;
      } else {
        crosses[point.key] = points[point.key]! + point.value;
      }
    }
  }

  return crosses;
}

void main() {
  var wires = File("input.txt").readAsLinesSync();
  var crossingPoints = getCrossingPoints(wires);

  var partOne = crossingPoints.keys.map((point) => point.x.abs() + point.y.abs()).reduce(min);
  print("Manhattan distance from the central port to the closest intersection (Part 1): $partOne");

  var partTwo = crossingPoints.values.reduce(min);
  print("Fewest combined steps the wires must take to reach an intersection (Part 2): $partTwo");
}
