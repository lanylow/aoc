package main

import (
	"bufio"
	"fmt"
	"io/ioutil"
	"os"
	"strconv"
	"strings"
)

// Make sure that the file has an empty line at the end, otherwise it will return incorrect results
func readFile(name string) string {
	file, _ := os.Open(name)
	text, _ := ioutil.ReadAll(bufio.NewReader(file))
	file.Close()
	return strings.TrimSuffix(string(text), "\r\n")
}

func main() {
	keys := strings.Split(readFile("input.txt"), "\r\n")

	doorPublicKey, _ := strconv.Atoi(keys[0])
	cardPublicKey, _ := strconv.Atoi(keys[1])

	transformNumber := 1
	cardLoopSize := 0

	for i := 0; cardLoopSize == 0; i++ {
		transformNumber = (transformNumber * 7) % 20201227

		if transformNumber == cardPublicKey {
			cardLoopSize = i + 1
		}
	}

	transformNumber = 1

	for i := 0; i < cardLoopSize; i++ {
		transformNumber = (transformNumber * doorPublicKey) % 20201227
	}

	fmt.Println("Encryption key the handshake trying to establish (Part 1):", transformNumber)
}
