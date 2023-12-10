/*
Strategy: 
1- Iterate every line
2- Find * symbols
3- Check previous, current and next line for numbers adjacent to the * symbol
4- If two numbers are found, multiply them together and add them to the total
*/

const fs = require("node:fs")
const input = fs.readFileSync("aoc_3_1_input.txt").toString()
const lines = input.split("\n")
let partNumberTotal = 0

// Iterate every lines once to handle the gear (asterix symbol)
for (const [lineIndex, currentLine] of lines.entries()) {
    const previousLine = lineIndex > 0 ? lines[lineIndex - 1] : ""
    const nextLine = lineIndex < lines.length - 1 ? lines[lineIndex + 1] : ""
    for (const [charIndex, char] of [...currentLine].entries()) {
        if (char != "*") {
            continue
        }
        const range = [charIndex - 1, charIndex, charIndex + 1]
        const numbersFound = findNumbersAdjacentToSymbol(range, [previousLine, currentLine, nextLine])
        if (numbersFound.length == 2) {
            partNumberTotal += numbersFound[0] * numbersFound[1]
        }
    }
}

console.log(`Part number total: ${partNumberTotal}`)

/**
 * @param {Array<number>} adjacentIndexList - The indexes before, at and after the symbol. Out of bound indexes such as -1 or line.length are supported. adjacentIndexList[1] is always the index of the symbol.
 * @param {Array<string>} lineList - An array with always three elements: previous, current and next line. Previous and next line can have a value of "".
 */
function findNumbersAdjacentToSymbol(adjacentIndexList, lineList) {
    const numbersFound = []
    // Search adjacent numbers in the previous, current and next line
    for (const line of lineList) {
        // The previous and next line can have two adjacent numbers if they are diagonal to the symbol with a dot in between them (ex: 123.47 with an asterix right above or below the dot)
        let hasNumberAtSymbolIndex = false
        // Search every adjacent index of the line for a digit
        for (const adjacentIndex of adjacentIndexList) {
            if (!isDigit(line[adjacentIndex])) {
                continue
            }
            // Digit found, now we need to build the full number
            let number = line[adjacentIndex]
            hasNumberAtSymbolIndex = adjacentIndex == adjacentIndexList[1]
            // Find every other digits before the adjacent digit by decrementing from the current index
            for (let i = 1; isDigit(line[adjacentIndex - i]); i++) {
                number = line[adjacentIndex - i] + number
                if (!hasNumberAtSymbolIndex) {
                    hasNumberAtSymbolIndex = adjacentIndex - i == adjacentIndexList[1]
                }
            }
            // Find every other digits after the adjacent digit
            for (let i = 1; isDigit(line[adjacentIndex + i]); i++) {
                number = number + line[adjacentIndex + i]
                if (!hasNumberAtSymbolIndex) {
                    hasNumberAtSymbolIndex = adjacentIndex + i == adjacentIndexList[1]
                }
            }
            // Push a NumberAdjacentToSymbol to the numbers found. We save the value, start index and line type
            numbersFound.push(number)
            // A number was found right above or below the symbol, thus there can only be one adjacent number on this line
            if (hasNumberAtSymbolIndex) {
                break
            }
        }
    }
    return numbersFound
}

/**
 * Simple function to assert if a char is a digit
 * @param {string} char 
 */
function isDigit(char) {
    return char >= "0" && char <= "9"
}
