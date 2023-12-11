/**
 * Strategy:
 * 1- Instanciate an empty array that will serve as a queue to keep track of the upcoming scratchcards copies
 * 2- Iterate every line
 * 3- Find the matching number count
 * 4- Dequeue the queue to get our current scratchcard copy count
 * 5- Add a value of the current scratchcard count or increment by the current scratchcard count the existing value of the queue
 * 6- Add the current scratchcard and its copy count to the total
 */

const fs = require("node:fs")
const input = fs.readFileSync("aoc_4_1_input.txt").toString()
const lines = input.split("\n")

let totalScratchcardCount = 0
// This array will act as queue; we dequeue it at the beginning of every iteration to see how many copies we have, and we push to it to add to the copies count of the next scratchcards
const copiesQueue = []
for (const line of lines) {
    // Count is at least one because of the original scratchcard
    let scratchcardCount = 1
    if (copiesQueue.length > 0) {
        // The first element of the queue always corresponds to our current scratchcard copy
        scratchcardCount += copiesQueue.shift()
    }
    // We can already add our current scratchcardCount to the total
    totalScratchcardCount += scratchcardCount
    // Find the matching number count to see if we have to update the copies queue
    const matchingNumberCount = getMatchingNumberCount(line)
    if (matchingNumberCount == 0) {
        // Nothing to do if there is no matching number
        continue
    }
    // Iterate the existing element of the queue or add new values
    for (let i = 0; i  < matchingNumberCount; i++) {
        if (copiesQueue.length > i) {
            // Elements exists, add the current scratchcard count
            copiesQueue[i] += scratchcardCount
        } else {
            // Create a new entry in the queue with the current scratchcard count
            copiesQueue.push(scratchcardCount)
        }
    }
}
console.log(`Total scratchcard count: ${totalScratchcardCount}`)

/**
 * Simple array intersect between the game numbers and winning numbwers to increments a count 
 */
function getMatchingNumberCount(line) {
    let count = 0
    for (const winningNumber of getWinningNumbers(line)) {
        for (const gameNumber of getGameNumbers(line)) {
            if (winningNumber == gameNumber) {
                count++
            }
        }
    }
    return count
}

/**
 * Parse a line of the input to get all the winning numbers
 */
function getWinningNumbers(line) {
    return line.substring(line.indexOf(":") + 1, line.indexOf("|")).split(" ").filter(el => el != "")
}

/**
 * Parse a line of the input to get all the game numbers
 */
function getGameNumbers(line) {
    return line.substring(line.indexOf("|") + 1).split(" ").filter(el => el != "")
}