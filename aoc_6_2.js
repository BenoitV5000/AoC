/*
Strategy
1- Build a race map by separating the input into columns
2- Test every possible time to beat the distance
3- Multiply the result with the current total
*/

const fs = require("node:fs")
const input = fs.readFileSync("aoc_6_1_input.txt").toString()
const lines = input.split("\n")

const raceTime = parseInt(getDataFromLines(lines[0]))
const distanceToBeat = parseInt(getDataFromLines(lines[1]))
let raceRecordRunsCount = 0
for (let buttonHeldTime = 1; buttonHeldTime < raceTime; buttonHeldTime++) {
    const speed = buttonHeldTime
    const timeRemaining = raceTime - buttonHeldTime
    const distance = speed * timeRemaining
    if (distance > distanceToBeat) {
        raceRecordRunsCount++
    }
}

console.log(`Total record runs count: ${raceRecordRunsCount}`)

function getDataFromLines(line) {
    while (true) {
        line = line.replace("  ", " ")
        if (line.indexOf("  ") == -1) {
            break
        }
    }
    
    const data = line.split(" ")
    
    data.splice(0, 1)

    return data.join("")
}