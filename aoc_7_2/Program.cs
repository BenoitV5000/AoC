/*
Strategy

Pretty much the same strategy as in p1, except when checking for types, see if we can change any J to increase the hand's power
*/

List<Card> ALL_CARD_LIST = [
    new(0, 'J'),
    new(1, '2'),
    new(2, '3'),
    new(3, '4'),
    new(4, '5'),
    new(5, '6'),
    new(6, '7'),
    new(7, '8'),
    new(8, '9'),
    new(9, 'T'),
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
long totalWinnings = 0;
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
    
    // Check if we have exactly one label.
    // J: Check if we have exactly two labels
    private static bool IsFiveOfAKind(Dictionary<char, int> labelCountDict) {
        bool baseCondition = labelCountDict.Count == 1;
        bool jCondition = labelCountDict.Count == 2;
        return ApplyConditions(labelCountDict, baseCondition, jCondition);
    }

    // Check if we have exactly two labels and if one of those label has four instances
    // J: Check if any of the labels have enough instances such as its instances plus the J instances equals four
    private static bool IsFourOfAKind(Dictionary<char, int> labelCountDict) {
        if (labelCountDict.Count == 2 && labelCountDict.ContainsValue(4)) {
            return true;
        }
        if (!labelCountDict.ContainsKey('J')) {
            return false;
        }
        foreach ((char label, int count) in labelCountDict) {
            if (label != 'J' && count + labelCountDict.GetValueOrDefault('J') == 4) {
                return true;
            }
        }
        return false;
    }

    // Check if we have exactly two labels and if one of those label has three instances (could also check two)
    // J: Check if we have exactly two labels with two instances
    private static bool IsFullHouse(Dictionary<char, int> labelCountDict) {
        bool baseCondition = labelCountDict.Count == 2 && labelCountDict.ContainsValue(3);
        bool jCondition = labelCountDict.Values.Count(el => el == 2) == 2;
        return ApplyConditions(labelCountDict, baseCondition, jCondition);
    }

    // Check if we have exactly three labels and if one of those label has three instances
    // J: Check if any of the labels, J included, have two instances
    private static bool IsThreeOfAKind(Dictionary<char, int> labelCountDict) {
        bool baseCondition = labelCountDict.Count == 3 && labelCountDict.ContainsValue(3);
        bool jCondition = labelCountDict.Values.Any(el => el == 2);
        return ApplyConditions(labelCountDict, baseCondition, jCondition);
    }

    // Check if we have exactly three labels and if two of those labels have two instances
    // J: Nothing to do since J can always give us a Three of a Kind when it can give us a Two Pair
    private static bool IsTwoPair(Dictionary<char, int> labelCountDict) {
        return labelCountDict.Count == 3 && labelCountDict.Values.Count(el => el == 2) == 2;
    }

    // Check if we have exactly four labels
    // J: Check if we have 5 different symbols
    private static bool IsOnePair(Dictionary<char, int> labelCountDict) {
        bool baseCondition = labelCountDict.Count == 4;
        bool jCondition = labelCountDict.Values.Count(el => el == 1) == 5;
        return ApplyConditions(labelCountDict, baseCondition, jCondition);
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

    private static bool ApplyConditions(Dictionary<char, int> labelCountDict, bool baseCondition, bool jCondition) {
        return baseCondition || labelCountDict.ContainsKey('J') && jCondition;
    }
}
