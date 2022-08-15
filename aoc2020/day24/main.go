package main

import (
	"bufio"
	"fmt"
	"io/ioutil"
	"os"
	"strings"
)

type Vector2D struct {
	x int
	y int
}

type HexGrid struct {
	minimums Vector2D
	maximums Vector2D
	data     map[string]bool
}

// Make sure that the file has an empty line at the end, otherwise it will return incorrect results
func readFile(name string) string {
	file, _ := os.Open(name)
	text, _ := ioutil.ReadAll(bufio.NewReader(file))
	file.Close()
	return strings.TrimSuffix(string(text), "\n")
}

func getMin(lhs int, rhs int) int {
	if lhs < rhs {
		return lhs
	}

	return rhs
}

func getMax(lhs int, rhs int) int {
	if lhs > rhs {
		return lhs
	}

	return rhs
}

func (hexGrid *HexGrid) getTileAt(x int, y int) bool {
	value, found := hexGrid.data[fmt.Sprintf("%d:%d", x, y)]

	if !found {
		return false
	}

	return value
}

func (hexGrid *HexGrid) setTileAt(x int, y int, value bool) {
	key := fmt.Sprintf("%d:%d", x, y)
	currentValue, found := hexGrid.data[key]

	if found && currentValue == value {
		return
	}

	if !found && value == false {
		return
	}

	if value == false {
		delete(hexGrid.data, key)
	} else {
		hexGrid.data[key] = value

		hexGrid.minimums.x = getMin(hexGrid.minimums.x, x)
		hexGrid.minimums.y = getMin(hexGrid.minimums.y, y)

		hexGrid.maximums.x = getMax(hexGrid.maximums.x, x)
		hexGrid.maximums.y = getMax(hexGrid.maximums.y, y)
	}
}

func createHexGrid() *HexGrid {
	return &HexGrid{data: map[string]bool{}}
}

func getBlackTiles(hexGrid *HexGrid) int {
	result := 0

	for i := hexGrid.minimums.x; i <= hexGrid.maximums.x; i++ {
		for j := hexGrid.minimums.y; j <= hexGrid.maximums.y; j++ {
			if hexGrid.getTileAt(i, j) {
				result += 1
			}
		}
	}

	return result
}

func getSolution(tiles string, part int) int {
	hexGrid := createHexGrid()

	for _, tile := range strings.Split(tiles, "\n") {
		x := 0
		y := 0

		for i := 0; i < len(tile)-1; {
			if tile[i] == 'e' {
				x += 1
				i += 1
			} else if tile[i] == 'w' {
				x -= 1
				i += 1
			} else if tile[i:i+2] == "se" {
				x += 1
				y -= 1
				i += 2
			} else if tile[i:i+2] == "sw" {
				y -= 1
				i += 2
			} else if tile[i:i+2] == "ne" {
				y += 1
				i += 2
			} else if tile[i:i+2] == "nw" {
				x -= 1
				y += 1
				i += 2
			}
		}

		hexGrid.setTileAt(x, y, !hexGrid.getTileAt(x, y))
	}

	if part == 1 {
		return getBlackTiles(hexGrid)
	}

	for day := 1; day <= 100; day++ {
		flippedHexGrid := createHexGrid()

		for i := hexGrid.minimums.x - 1; i <= hexGrid.maximums.x+1; i++ {
			for j := hexGrid.minimums.y - 1; j <= hexGrid.maximums.y+1; j++ {
				adjacentCount := 0

				if hexGrid.getTileAt(i+1, j) {
					adjacentCount += 1
				}

				if hexGrid.getTileAt(i-1, j) {
					adjacentCount += 1
				}

				if hexGrid.getTileAt(i, j+1) {
					adjacentCount += 1
				}

				if hexGrid.getTileAt(i, j-1) {
					adjacentCount += 1
				}

				if hexGrid.getTileAt(i+1, j-1) {
					adjacentCount += 1
				}

				if hexGrid.getTileAt(i-1, j+1) {
					adjacentCount += 1
				}

				state := hexGrid.getTileAt(i, j)

				if state && (adjacentCount == 0 || adjacentCount > 2) {
					flippedHexGrid.setTileAt(i, j, !state)
				} else if !state && adjacentCount == 2 {
					flippedHexGrid.setTileAt(i, j, !state)
				} else {
					flippedHexGrid.setTileAt(i, j, state)
				}
			}
		}

		hexGrid = flippedHexGrid
	}

	return getBlackTiles(hexGrid)
}

func main() {
	tiles := readFile("input.txt")
	fmt.Println("Amount of tiles left with the black side up (Part 1):", getSolution(tiles, 1))
	fmt.Println("Amount of tiles left with the black side up after 100 days (Part 2):", getSolution(tiles, 2))
}
