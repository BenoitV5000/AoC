/*
Strategy
1- Iterate through each lines
2- Save all the winning numbers
3- Count how many winning numbers are in our numbers
4- Add this count to our point sum
*/

const fs = require("node:fs")

const input: string = fs.readFileSync("aoc_4_1_input.txt").toString()

const lines: string[] = input.split("\n")

let pointSum = 0

for (const line of lines) {
    const winningNumbers: string[] = getWinningNumbers(line)
    const gameNumbers: string[] = getGameNumbers(line)
    let points: number = 0
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

function getWinningNumbers(line: string): string[] {
    return line.substring(line.indexOf(":"), line.indexOf("|")).trim().split(" ")
}

function getGameNumbers(line: string): string[] {
    return line.substring(line.indexOf("|")).trim().split(" ")
}