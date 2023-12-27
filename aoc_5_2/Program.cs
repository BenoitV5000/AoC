/*
Strategy

Did some tests with the part 1 solution to see if I could bruteforce p2 with the same algorithm... it would take 46 days to go through the almost 2 billions seeds, so thats out of the window.
Instead, I'll start from location 0 and work my way in reverse until we hit a seed that exists in the range.

1- Parse the input to get every possible seed numbers
2- Parse the input to get in memory every mapping from the destination to the source and the direction of the mapping
3- Iterate starting from the lowest location number possible (0)
4- Work backwards through the conversion list to find the lowest seed number
*/

using System.Diagnostics;

const long MAX_ITERATION = 10000000000;
const string SEEDS = "seeds:";
ConversionPair[] conversionPairList = {
    new("humidity-to-location map:", ""),
    new("temperature-to-humidity map:", "humidity-to-location map:"),
    new("light-to-temperature map:", "temperature-to-humidity map:"),
    new("water-to-light map:", "light-to-temperature map:"),
    new("fertilizer-to-water map:", "water-to-light map:"),
    new("soil-to-fertilizer map:", "fertilizer-to-water map:"),
    new("seed-to-soil map:", "soil-to-fertilizer map:")
};

string input = "";
using (StreamReader sr = new StreamReader("./input.txt")) {
    input = sr.ReadToEnd();
}

string[] seedList = input.Substring(input.IndexOf(SEEDS) + SEEDS.Length, input.IndexOf('\n') - input.IndexOf(SEEDS) - SEEDS.Length).Trim().Split(' ');
List<SeedRange> seedRangeList = getSeedRangeList(seedList);

List<List<List<string>>> conversionMapList = [];
foreach (ConversionPair conversionPair in conversionPairList) {
    conversionMapList.Add(getConversionMap(conversionPair.sourceHeader, conversionPair.destinationHeader));
}

Stopwatch stopwatch = new();
stopwatch.Start();

for (long i = 0; i < MAX_ITERATION; i++) {
    long currentSource = i;
    foreach (List<List<string>> conversionMap in conversionMapList) {
        foreach (List<string> conversion in conversionMap) {
            if (currentSource >= long.Parse(conversion[0]) && currentSource < long.Parse(conversion[0]) + long.Parse(conversion[2])) {
                currentSource = long.Parse(conversion[1]) + currentSource - long.Parse(conversion[0]);
                break;
            }
        }
    }
    foreach (SeedRange seedRange in seedRangeList) {
        if (currentSource >= seedRange.from && currentSource <= seedRange.to) {
            Console.WriteLine($"The lowest location is {i} and the seed number is {currentSource}");
            Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds / 1000f} seconds");
            return;
        }
    }
    if (i != 0 && i % 1000000 == 0) {
        Console.WriteLine($"At iteration {i}. Elapsed time: {stopwatch.ElapsedMilliseconds / 1000f} seconds");
    }
}

Console.WriteLine("Could not find any seed number");
Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds / 1000f} seconds");




























/*foreach (string seed in seedList) {
    long previousDestination = long.Parse(seed);
    foreach (ConversionPair conversionPair in conversionPairList) {
        List<List<string>> conversionMap = getConversionMap(conversionPair.sourceHeader, conversionPair.destinationHeader);
        foreach (List<string> conversion in conversionMap) {
            long destination = long.Parse(conversion[0]);
            long source = long.Parse(conversion[1]);
            long range = long.Parse(conversion[2]);
            if (previousDestination >= source && previousDestination < source + range) {
                previousDestination = destination + previousDestination - source;
                break;
            }
        }
    }
    if (lowestLocationNumber == -1 || previousDestination < lowestLocationNumber) {
        lowestLocationNumber = previousDestination;
    }
}
Console.WriteLine($"Lowest location number: {lowestLocationNumber}");*/



List<List<string>> getConversionMap(string sourceCategoryHeader, string destinationCategoryHeader) {
    List<List<string>> map = [];
    string conversionList;
    if (destinationCategoryHeader != "") {
        conversionList = input.Substring(
            input.IndexOf(sourceCategoryHeader) + sourceCategoryHeader.Length,
            input.IndexOf(destinationCategoryHeader) - input.IndexOf(sourceCategoryHeader) - sourceCategoryHeader.Length).Trim();
    } else {
        conversionList = input.Substring(input.IndexOf(sourceCategoryHeader) + sourceCategoryHeader.Length).Trim();
    }
    foreach (string conversion in conversionList.Split('\n')) {
        map.Add([.. conversion.Split(' ')]);
    }
    return map;
}

List<SeedRange> getSeedRangeList(string[] seedList) {
    List<SeedRange> seedRangeList = [];
    SeedRange currentSeedRange = new();
    for (int i = 0; i < seedList.Length; i++) {
        if (i % 2 == 0) {
            currentSeedRange = new SeedRange
            {
                from = long.Parse(seedList[i])
            };
        } else {
            currentSeedRange.to = currentSeedRange.from + long.Parse(seedList[i]) - 1;
            seedRangeList.Add(currentSeedRange);
        }
    }
    return seedRangeList;
}



class ConversionPair {
    public string sourceHeader;
    public string destinationHeader;

    public ConversionPair(string sourceHeader, string destinationHeader) {
        this.sourceHeader = sourceHeader;
        this.destinationHeader = destinationHeader;
    }
}

class SeedRange {
    public long from;
    public long to;

    public SeedRange(long from, long to) {
        this.from = from;
        this.to = to;
    }

    public SeedRange() {}
}
