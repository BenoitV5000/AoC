/*
Strategy
1- Iterate through each lines
2- Save all the winning numbers
3- Count how many winning numbers are in our numbers
4- Add this count to our point sum
*/

const fs = require("node:fs")

const input = fs.readFileSync("aoc_4_1_input.txt").toString()

const lines = input.split("\n")

let pointSum = 0

for (const line of lines) {
    const winningNumbers = getWinningNumbers(line)
    const gameNumbers = getGameNumbers(line)
    let points = 0
    for (const winningNumber of winningNumbers) {
        for (const gameNumber of gameNumbers) {
            if (winningNumber == gameNumber) {
                points = points == 0 ? 1 : points * 2
            }
        }
    }
    pointSum += points
}

console.log(`Point Sum: ${pointSum}`)

function getWinningNumbers(line) {
    return line.substring(line.indexOf(":") + 1, line.indexOf("|")).split(" ").filter(el => el != "")
}

function getGameNumbers(line) {
    return line.substring(line.indexOf("|") + 1).split(" ").filter(el => el != "")
}