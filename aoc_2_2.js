const fs = require("node:fs")

const input = fs.readFileSync("aoc_2_2_input.txt").toString()

let cubePowerSum = 0

const lines = input.split("\n")

for (const line of lines) {
    cubePowerSum += findMinimumPossible(line, "red") * findMinimumPossible(line, "green") * findMinimumPossible(line, "blue")
}

console.log(`Final power sum: ${cubePowerSum}`)

function findMinimumPossible(line, color) {
    let minimum = -1
    let colorIndex = -1
    while (true) {
        colorIndex = line.indexOf(color, colorIndex + 1)
        if (colorIndex == -1) {
            break
        }
        let value = ""
        for (let valueIndex = colorIndex - 2; line.charAt(valueIndex) >= "0" && line.charAt(valueIndex) <= "9"; valueIndex--) {
            value = line.charAt(valueIndex) + value
        }
        value = parseInt(value)
        if (minimum == -1 || value > minimum) {
            minimum = value
        }
    }
    return minimum == -1 ? 0 : minimum
}


