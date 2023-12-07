const fs = require("node:fs")

const input = fs.readFileSync("aoc_2_1_input.txt").toString()

const RED_CUBE = {"color": "red", "max": 12}
const GREEN_CUBE = {"color": "green", "max": 13}
const BLUE_CUBE = {"color": "blue", "max": 14}

const CUBES = [RED_CUBE, GREEN_CUBE, BLUE_CUBE]

let idSum = 0

const lines = input.split("\n")

for (const line of lines) {
    let isValid = true
    for (const cube of CUBES) {
        if (!hasCountsBelowMax(line, cube.color, cube.max)) {
            isValid = false
            break
        }
    }
    if (!isValid) {
        continue
    }
    const id = parseInt(line.substring(line.indexOf(" ") + 1, line.indexOf(":")))
    idSum += parseInt(id)
    console.log(idSum)
}

console.log(`Final sum: ${idSum}`)

function hasCountsBelowMax(line, color, max) {
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
        if (parseInt(value) > max) {
            return false
        }
    }
    return true
}


