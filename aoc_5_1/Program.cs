/*
Strategy
1- Parse the input to get every seed number in then iterate on them
2- Create source - destination pairs on iterate on them
3- Find all the source values for which previousDestination >= source && previousDestination < source + range
*/

const string SEEDS = "seeds:";

ConversionPair[] conversionPairList = {
    new("seed-to-soil map:", "soil-to-fertilizer map:"),
    new("soil-to-fertilizer map:", "fertilizer-to-water map:"),
    new("fertilizer-to-water map:", "water-to-light map:"),
    new("water-to-light map:", "light-to-temperature map:"),
    new("light-to-temperature map:", "temperature-to-humidity map:"),
    new("temperature-to-humidity map:", "humidity-to-location map:"),
    new("humidity-to-location map:", ""),
};

string input = "";
using (StreamReader sr = new StreamReader("./input.txt")) {
    input = sr.ReadToEnd();
}

long lowestLocationNumber = -1;
string[] seeds = input.Substring(input.IndexOf(SEEDS) + SEEDS.Length, input.IndexOf('\n') - input.IndexOf(SEEDS) - SEEDS.Length).Trim().Split(' ');
foreach (string seed in seeds) {
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
Console.WriteLine($"Lowest location number: {lowestLocationNumber}");



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

class ConversionPair {
    public string sourceHeader;
    public string destinationHeader;

    public ConversionPair(string sourceHeader, string destinationHeader) {
        this.sourceHeader = sourceHeader;
        this.destinationHeader = destinationHeader;
    }
}

