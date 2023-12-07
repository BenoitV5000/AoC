/*
Strategy: 
1- Iterate every line
2- Build a map of each number and their start index in that line
3- Use the start index and line length to look at the current, previous and next line to see if there is a symbol
*/

const fs = require("node:fs")

const input = fs.readFileSync("aoc_3_1_input.txt").toString()

const lines = input.split("\n")

let partNumberSum = 0

for (const [index, currentLine] of lines.entries()) {
    // Build a map of all the numbers in the line with their start index
    const numbersWithIndex = findNumbersWithIndex(currentLine)
    const previousLine = index > 0 ? lines[index - 1] : ""
    const nextLine = index < lines.length - 1 ? lines[index + 1] : ""
    // Look at the previous and next line to see if there is an adjacent symbol
    for (const [partNumberIndex, partNumber] of numbersWithIndex.entries()) {
        if (hasAdjacentSymbol(currentLine, previousLine, nextLine, partNumberIndex, partNumber)) {
            partNumberSum += parseInt(partNumber)
        }
    }
}

console.log(`Part Number Sum: ${partNumberSum}`)

function isSymbol(char) {
    return !(char >= "a" && char <= "z") &&
    !(char >= "A" && char <= "Z") &&
    !(char >= "0" && char <= "9") &&
    char != "." && char != ""
}

function findNumbersWithIndex(line) {
    const numbersWithIndex = new Map()
    let number = ""
    for (const [index, char] of [...line].entries()) {
        // Digit is found, keep building the number
        if (char >= "0" && char <= "9") {
            number = number + char
            continue
        }
        // Digit not found, push number to list if we were building one and reset number
        if (number != "") {
            numbersWithIndex.set(index - number.length, number)
            number = ""
        }
    }
    // In case there is a number at the end of a line
    if (number != "") {
        numbersWithIndex.set(line.length - number.length, number)
    }
    return numbersWithIndex
}

function hasAdjacentSymbol(currentLine, previousLine, nextLine, index, number) {
    // Because of diagonals, the index range to check in the adjacent lines is minus 1 for start and plus 1 for end
    const startIndex = index - 1
    const endIndex = index + number.length + 1
    // For the current line, check only the at the beginning and the and of the range
    if (isSymbol(currentLine.charAt(startIndex)) || isSymbol(currentLine.charAt(endIndex - 1))) {
        return true
    }
    // For the previous and next line, check the whole range
    for (const line of new Array(previousLine, nextLine)) {
        const rangeToCheck = line.substring(startIndex, endIndex)
        for (const char of rangeToCheck) {
            if (isSymbol(char)) {
                return true
            }
        }
    }
    return false
}
