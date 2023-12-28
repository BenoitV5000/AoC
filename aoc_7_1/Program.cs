/*
Strategy

1- Create a Comparer which will contain the type rules to compare cards
2- Parse the input to get a list of Hand values
3- Iterate every hands and add them to a sorted set
4- Iterate the sorted list to calculate the total winnings
*/

List<Card> ALL_CARD_LIST = [
    new(0, '2'),
    new(1, '3'),
    new(2, '4'),
    new(3, '5'),
    new(4, '6'),
    new(5, '7'),
    new(6, '8'),
    new(7, '9'),
    new(8, 'T'),
    new(9, 'J'),
    new(10, 'Q'),
    new(11, 'K'),
    new(12, 'A')
];

var input = "";
using (StreamReader sr = new("./input.txt")) {
    input = sr.ReadToEnd();
}
List<Hand> handList = parseInputToHandList(input);
SortedSet<Hand> handsInWinningOrder = new(new HandComparer());
foreach (Hand hand in handList) {
    handsInWinningOrder.Add(hand);
}
var totalWinnings = 0;
var ranking = 1;
foreach (Hand hand in handsInWinningOrder) {
    totalWinnings += hand.Bid * ranking++;
}
Console.WriteLine($"Total winnings: {totalWinnings}");

List<Hand> parseInputToHandList(string input)
{
    List<Hand> handList = [];
    foreach(var rawHand in input.Split("\n")) {
        var rawLabelList = rawHand.Split(" ")[0];
        var rawBidStr = rawHand.Split(" ")[1];
        handList.Add(new(parseRawLabelListToCard(rawLabelList), int.Parse(rawBidStr)));
    }
    return handList;
}

List<Card> parseRawLabelListToCard(string rawLabelStr) {
    List<Card> cardList = [];
    foreach(var character in rawLabelStr) {
        cardList.Add(ALL_CARD_LIST.Find(el => el.Label == character));
    }
    return cardList;
}

readonly struct Card(int power, char label)
{
    public int Power { get; init; } = power;
    public char Label { get; init; } = label;
}

readonly struct Hand(List<Card> cardList, int bid)
{
    public List<Card> CardList { get; init; } = cardList;
    public int Bid { get; init; } = bid;
}

class HandComparer : IComparer<Hand>
{
    const int HAND_CARD_LIST_LENGTH = 5;
    readonly List<Predicate<Dictionary<char, int>>> ORDERED_TYPE_RULE_LIST = [
        IsFiveOfAKind,
        IsFourOfAKind,
        IsFullHouse,
        IsThreeOfAKind,
        IsTwoPair,
        IsOnePair
    ];
    public int Compare(Hand x, Hand y)
    {
        var xLabelCountDict = GetLabelCountDict(x);
        var yLabelCountDict = GetLabelCountDict(y);
        foreach (Predicate<Dictionary<char, int>> typeRule in ORDERED_TYPE_RULE_LIST)
        {
            
            var xIsType = typeRule.Invoke(xLabelCountDict);
            var yIsType = typeRule.Invoke(yLabelCountDict);
            if (xIsType && !yIsType) {
                return 1;
            }
            if (!xIsType && yIsType) {
                return -1;
            }
            if (xIsType && yIsType) {
                break;
            }
        }
        for (var i = 0; i < HAND_CARD_LIST_LENGTH; i++) {
            if (x.CardList[i].Power > y.CardList[i].Power) {
                return 1;
            }
            if (x.CardList[i].Power < y.CardList[i].Power) {
                return -1;
            }
        }
        return 0;
    }

    private static Dictionary<char, int> GetLabelCountDict(Hand hand) {
        var labelCountDict = new Dictionary<char, int>();
        foreach (char label in hand.CardList.Select(card => card.Label)) {
            if (!labelCountDict.TryAdd(label, 1)) {
                labelCountDict[label]++;
            }
        }
        return labelCountDict;
    }

    // Check if we have exactly one label
    private static bool IsFiveOfAKind(Dictionary<char, int> labelCountDict) {
        return labelCountDict.Count == 1;
    }

    // Check if we have exactly two labels and if one of those label has four instances
    private static bool IsFourOfAKind(Dictionary<char, int> labelCountDict) {
        return labelCountDict.Count == 2 && labelCountDict.ContainsValue(4);
    }

    // Check if we have exactly two labels and if one of those label has three instances (could also check two)
    private static bool IsFullHouse(Dictionary<char, int> labelCountDict) {
        return labelCountDict.Count == 2 && labelCountDict.ContainsValue(3);
    }

    // Check if we have exactly three labels and if one of those label has three instances
    private static bool IsThreeOfAKind(Dictionary<char, int> labelCountDict) {
        return labelCountDict.Count == 3 && labelCountDict.ContainsValue(3);
    }

    // Check if we have exactly three labels and if two of those labels have two instances
    private static bool IsTwoPair(Dictionary<char, int> labelCountDict) {
        return labelCountDict.Count == 3 && labelCountDict.Values.Count(el => el == 2) == 2;
    }

    // Check if we have exactly four labels
    private static bool IsOnePair(Dictionary<char, int> labelCountDict) {
        return labelCountDict.Count == 4;
    }
}
